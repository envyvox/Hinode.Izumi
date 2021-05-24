using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.TransitCommands
{
    [CommandCategory(CommandCategory.Transit)]
    [IzumiRequireRegistry]
    public class TransitListCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteRecord> _emotes;

        public TransitListCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }


        [Command("отправления"), Alias("transits")]
        [Summary("Посмотреть доступные отправления из текущей локации")]
        public async Task TransitListTask()
        {
            // получаем информацию о перемещении пользователя
            var userMovement = await _mediator.Send(new GetUserMovementQuery((long) Context.User.Id));

            // если пользователь находится в пути, он не может просматривать отправления
            if (userMovement is not null)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.TransitMakeAlready.Parse(
                    userMovement.Destination.Localize())));
            }
            else
            {
                // получаем иконки из базы
                _emotes = await _mediator.Send(new GetEmotesQuery());
                // получаем пользователя из базы
                var user = await _mediator.Send(new GetUserByIdQuery((long) Context.User.Id));
                // получаем список перемещений доступных из локации пользователя
                var transits = await _mediator.Send(new GetTransitsFromLocationQuery(user.Location));
                // получаем мастерство торговли пользователя
                var userMastery = await _mediator.Send(new GetUserMasteryQuery(
                    (long) Context.User.Id, Mastery.Trading));
                // округляем мастерство пользователя
                var userRoundedMastery = (long) Math.Round(userMastery.Amount);

                var embed = new EmbedBuilder()
                    // баннер перемещения
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.TransitList)))
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
                                ? await _mediator.Send(new GetTransitCostWithDiscountQuery(
                                    userRoundedMastery, transit.Price))
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
                    var transitTime = await _mediator.Send(new GetActionTimeQuery(transit.Time, user.Energy));

                    // получаем текущее событие
                    var currentEvent = (Event) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentEvent));
                    // если сейчас проходит событие
                    if (currentEvent != Event.None)
                        // то нужно ускорить отправление
                        transitTime -= transitTime *
                            // получаем % ускорения перемещения во время события
                            await _mediator.Send(new GetPropertyValueQuery(Property.EventReduceTransitTime)) / 100;

                    // заполняем строку длительности перемещения в зависимости от его длительности
                    var transitTimeString = transit.Time == transitTime
                        ? transit.Time.Minutes().Humanize(1, new CultureInfo("ru-RU"))
                        : $"~~{transit.Time.Minutes().Humanize(1, new CultureInfo("ru-RU"))}~~ {transitTime.Minutes().Humanize(1, new CultureInfo("ru-RU"))}";

                    embed.AddField(IzumiReplyMessage.TransitListFieldName.Parse(
                            _emotes.GetEmoteOrBlank("List"), transit.Destination.GetHashCode(),
                            transit.Destination.Localize()),
                        IzumiReplyMessage.TransitListFieldDesc.Parse(transitTimeString, transitCostString));
                }

                await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));

                // проверяем нужно ли двинуть прогресс обучения пользователя
                await _mediator.Send(new CheckUserTutorialStepCommand(
                    (long) Context.User.Id, TutorialStep.CheckTransits));

                await Task.CompletedTask;
            }
        }
    }
}
