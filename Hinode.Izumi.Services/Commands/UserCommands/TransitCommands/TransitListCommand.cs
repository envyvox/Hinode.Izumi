using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.EmoteService.Models;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.MasteryService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.TransitCommands
{
    [CommandCategory(CommandCategory.Transit)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class TransitListCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly ILocationService _locationService;
        private readonly IUserService _userService;
        private readonly IEmoteService _emoteService;
        private readonly ICalculationService _calc;
        private readonly IMasteryService _masteryService;
        private readonly ILocalizationService _local;
        private readonly ITrainingService _trainingService;
        private readonly IImageService _imageService;
        private readonly IPropertyService _propertyService;

        private Dictionary<string, EmoteModel> _emotes;

        public TransitListCommand(IDiscordEmbedService discordEmbedService, ILocationService locationService,
            IUserService userService, IEmoteService emoteService, ICalculationService calc,
            IMasteryService masteryService, ILocalizationService local, ITrainingService trainingService,
            IImageService imageService, IPropertyService propertyService)
        {
            _discordEmbedService = discordEmbedService;
            _locationService = locationService;
            _userService = userService;
            _emoteService = emoteService;
            _calc = calc;
            _masteryService = masteryService;
            _local = local;
            _trainingService = trainingService;
            _imageService = imageService;
            _propertyService = propertyService;
        }

        [Command("отправления"), Alias("transits")]
        [Summary("Посмотреть доступные отправления из текущей локации")]
        public async Task TransitListTask()
        {
            // получаем информацию о перемещении пользователя
            var userMovement = await _locationService.GetUserMovement((long) Context.User.Id);

            // если пользователь находится в пути, он не может просматривать отправления
            if (userMovement.Id != 0)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.TransitMakeAlready.Parse(
                    userMovement.Destination.Localize())));
            }
            else
            {
                // получаем иконки из базы
                _emotes = await _emoteService.GetEmotes();
                // получаем пользователя из базы
                var user = await _userService.GetUser((long) Context.User.Id);
                // получаем список перемещений доступных из локации пользователя
                var transits = await _locationService.GetTransit(user.Location);
                // получаем мастерство торговли пользователя
                var userMastery = await _masteryService.GetUserMastery((long) Context.User.Id, Mastery.Trading);
                // округляем мастерство пользователя
                var userRoundedMastery = (long) Math.Round(userMastery.Amount);

                var embed = new EmbedBuilder()
                    // баннер перемещения
                    .WithImageUrl(await _imageService.GetImageUrl(Image.TransitList))
                    // рассказываем как отправиться куда-то
                    .WithDescription(
                        IzumiReplyMessage.TransitListDesc.Parse(
                            _emotes.GetEmoteOrBlank("Energy"), _emotes.GetEmoteOrBlank(Mastery.Trading.ToString()),
                            Mastery.Trading.Localize()) +
                        $"\n{_emotes.GetEmoteOrBlank("Blank")}");

                // создаем embed field для всех возможных перемещений
                foreach (var transit in transits)
                {
                    // определяем стоимость перемещения
                    var transitCost =
                        transit.Price > 0
                            ? userRoundedMastery > 50
                                ? await _calc.TransitCostWithDiscount(userRoundedMastery, transit.Price)
                                : transit.Price
                            : 0;
                    // заполняем строку стоимости перемещения в зависимости от его стоимости
                    var transitCostString =
                        transit.Price > 0
                            ? userRoundedMastery > 50
                                ? $"{_emotes.GetEmoteOrBlank(Currency.Ien.ToString())} ~~{transit.Price}~~ {transitCost} {_local.Localize(Currency.Ien.ToString(), transitCost)}"
                                : $"{_emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {transitCost} {_local.Localize(Currency.Ien.ToString(), transitCost)}"
                            : $"{_emotes.GetEmoteOrBlank(Currency.Ien.ToString())} бесплатно";

                    // определяем длительность перемещения
                    var transitTime = _calc.ActionTime(transit.Time, user.Energy);

                    // получаем текущее событие
                    var currentEvent = (Event) await _propertyService.GetPropertyValue(Property.CurrentEvent);
                    // если сейчас проходит событие
                    if (currentEvent != Event.None)
                        // то нужно ускорить отправление
                        transitTime -= transitTime *
                            // получаем % ускорения перемещения во время события
                            await _propertyService.GetPropertyValue(Property.EventReduceTransitTime) / 100;

                    // заполняем строку длительности перемещения в зависимости от его длительности
                    var transitTimeString = transit.Time == transitTime
                        ? transit.Time.Minutes().Humanize(1, new CultureInfo("ru-RU"))
                        : $"~~{transit.Time.Minutes().Humanize(1, new CultureInfo("ru-RU"))}~~ {transitTime.Minutes().Humanize(1, new CultureInfo("ru-RU"))}";

                    embed.AddField(IzumiReplyMessage.TransitListFieldName.Parse(
                            _emotes.GetEmoteOrBlank("List"), transit.Destination.GetHashCode(),
                            transit.Destination.Localize()),
                        IzumiReplyMessage.TransitListFieldDesc.Parse(transitTimeString, transitCostString));
                }

                await _discordEmbedService.SendEmbed(Context.User, embed);
                // проверяем нужно ли двинуть прогресс обучения пользователя
                await _trainingService.CheckStep((long) Context.User.Id, TrainingStep.CheckTransits);
                await Task.CompletedTask;
            }
        }
    }
}
