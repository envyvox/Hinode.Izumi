using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.MasteryService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.SeedService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuySeedCommand : IShopBuySeedCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _local;
        private readonly IInventoryService _inventoryService;
        private readonly ISeedService _seedService;
        private readonly IMasteryService _masteryService;
        private readonly ICalculationService _calc;
        private readonly IPropertyService _propertyService;

        public ShopBuySeedCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IImageService imageService, ILocalizationService local, IInventoryService inventoryService,
            ISeedService seedService, IMasteryService masteryService, ICalculationService calc,
            IPropertyService propertyService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _imageService = imageService;
            _local = local;
            _inventoryService = inventoryService;
            _seedService = seedService;
            _masteryService = masteryService;
            _calc = calc;
            _propertyService = propertyService;
        }

        public async Task Execute(SocketCommandContext context, long seedId, long amount)
        {
            // получаем желаемое семя
            var seed = await _seedService.GetSeed(seedId);
            // получаем текущий сезон
            var season = (Season) await _propertyService.GetPropertyValue(Property.CurrentSeason);

            // проверяем подходит ли сезон семени для покупки
            if (seed.Season != season)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ShopBuySeedWrongSeason.Parse(
                    seed.Season.Localize())));
            }
            else
            {
                // получаем иконки из базы
                var emotes = await _emoteService.GetEmotes();
                // получаем валюту пользователя
                var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id, Currency.Ien);
                // получаем мастерсто торговли пользователя
                var userMastery = await _masteryService.GetUserMastery((long) context.User.Id, Mastery.Trading);
                // определяем стоимость семени после скидки мастерства торговли
                var seedPrice = await _calc.SeedPriceWithDiscount(
                    // округяем мастерство пользователя
                    (long) Math.Floor(userMastery.Amount), seed.Price);

                // проверяем хватает ли пользователю денег на оплату семян
                if (userCurrency.Amount < seedPrice * amount)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyNoCurrency.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                        _local.Localize(Currency.Ien.ToString(), 5))));
                }
                else
                {
                    // забираем у пользователя деньги на оплату семян
                    await _inventoryService.RemoveItemFromUser(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                        seedPrice * amount);
                    // добавляем пользователю семена
                    await _inventoryService.AddItemToUser(
                        (long) context.User.Id, InventoryCategory.Seed, seed.Id, amount);

                    var embed = new EmbedBuilder()
                        // баннер магазина семян
                        .WithImageUrl(await _imageService.GetImageUrl(Image.ShopSeed))
                        // подверждаем успешную покупку семян
                        .WithDescription(IzumiReplyMessage.ShopBuySeedSuccess.Parse(
                            emotes.GetEmoteOrBlank(seed.Name), amount, _local.Localize(seed.Name, amount),
                            emotes.GetEmoteOrBlank(Currency.Ien.ToString()), seedPrice * amount,
                            _local.Localize(Currency.Ien.ToString(), seedPrice * amount)));

                    await _discordEmbedService.SendEmbed(context.User, embed);
                    await Task.CompletedTask;
                }
            }
        }
    }
}
