using System;
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
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.MasteryService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.TransitCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class TransitMakeCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IPropertyService _propertyService;
        private readonly ILocationService _locationService;
        private readonly IUserService _userService;
        private readonly IMasteryService _masteryService;
        private readonly ICalculationService _calc;
        private readonly IInventoryService _inventoryService;
        private readonly ILocalizationService _local;
        private readonly IImageService _imageService;

        public TransitMakeCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IPropertyService propertyService, ILocationService locationService, IUserService userService,
            IMasteryService masteryService, ICalculationService calc, IInventoryService inventoryService,
            ILocalizationService local, IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _propertyService = propertyService;
            _locationService = locationService;
            _userService = userService;
            _masteryService = masteryService;
            _calc = calc;
            _inventoryService = inventoryService;
            _local = local;
            _imageService = imageService;
        }

        [Command("отправиться"), Alias("transit")]
        public async Task TransitMakeTask(Location destination = 0)
        {
            // получаем пользователя из базы
            var user = await _userService.GetUser((long) Context.User.Id);
            // получаем данные о перемещении пользователя
            var userMovement = await _locationService.GetUserMovement((long) Context.User.Id);

            // если пользователь находится в процессе перемещения - он не может отправиться
            if (userMovement.Id != 0)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.TransitMakeAlready.Parse(
                    userMovement.Destination.Localize())));
            }
            // проверяем что пользователь указал точку назначения
            else if (destination == Location.InTransit)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.TransitMakeNull.Parse()));
            }
            // точка назначения не может быть текущей локацией пользователя
            else if (destination == user.Location)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.TransitMakeCurrent.Parse()));
            }
            else
            {
                // получаем иконки из базы
                var emotes = await _emoteService.GetEmotes();
                // получаем информацию о перемещении
                var transit = await _locationService.GetTransit(user.Location, destination);
                // получаем мастерство торговли пользователя
                var userMastery = await _masteryService.GetUserMastery((long) Context.User.Id, Mastery.Trading);
                // определяем стоимость перемещения
                var transitCost = userMastery.Amount > 50
                    ? await _calc.TransitCostWithDiscount(
                        (long) Math.Floor(userMastery.Amount), transit.Price)
                    : transit.Price;
                // получаем валюту пользователя
                var userCurrency = await _inventoryService.GetUserCurrency((long) Context.User.Id, Currency.Ien);

                // проверяем хватает ли пользователю на оплату перемещения
                if (userCurrency.Amount < transitCost)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.TransitMakeNoCurrency.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), transitCost)));
                }
                else
                {
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

                    // начинаем транзит
                    await _locationService.StartUserTransit(
                        (long) Context.User.Id, user.Location, destination, transitTime);

                    var embed = new EmbedBuilder()
                        // баннер перемещения
                        .WithImageUrl(await _imageService.GetImageUrl(Image.InTransit))
                        // подверждаем что пемерещение успешно начато
                        .WithDescription(
                            IzumiReplyMessage.TransitMakeSuccess.Parse(
                                emotes.GetEmoteOrBlank(Currency.Ien.ToString()), transitCost,
                                destination.Localize(), transitTime) +
                            $"\n{emotes.GetEmoteOrBlank("Blank")}")
                        // длительность
                        .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                            transitTime.Minutes().Humanize(2, new CultureInfo("ru-RU")), true);

                    // если перемещение было не в подлокацию
                    if (!destination.SubLocation())
                    {
                        // забираем у пользователя деньги на оплату перемещения
                        await _inventoryService.RemoveItemFromUser(
                            (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                            transitCost);
                        // забираем у пользователя энергию за перемещение
                        await _userService.RemoveEnergyFromUser((long) Context.User.Id,
                            await _propertyService.GetPropertyValue(Property.EnergyCostTransit));
                        // добавляем embed field с информацией об оплате
                        embed.AddField(IzumiReplyMessage.TransitCostFieldName.Parse(),
                            $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {transitCost} {_local.Localize(Currency.Ien.ToString(), transitCost)}",
                            true);
                    }

                    await _discordEmbedService.SendEmbed(Context.User, embed);
                    await Task.CompletedTask;
                }
            }
        }
    }
}
