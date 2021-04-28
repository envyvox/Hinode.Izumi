using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.BannerService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuyBannerCommand : IShopBuyBannerCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly IBannerService _bannerService;
        private readonly IInventoryService _inventoryService;
        private readonly IImageService _imageService;

        public ShopBuyBannerCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocalizationService local, IBannerService bannerService, IInventoryService inventoryService,
            IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _local = local;
            _bannerService = bannerService;
            _inventoryService = inventoryService;
            _imageService = imageService;
        }

        public async Task Execute(SocketCommandContext context, long bannerId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем желаемый баннер из сменяемого магазина
            var banner = await _bannerService.GetDynamicShopBanner(bannerId);
            // получаем валюту пользователя
            var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id, Currency.Ien);

            var embed = new EmbedBuilder()
                // баннер магазина баннеров
                .WithImageUrl(await _imageService.GetImageUrl(Image.ShopBanner));

            // проверяем хватит ли пользователю денег на оплату баннера
            if (userCurrency.Amount < banner.Price)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyNoCurrency.Parse(
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), _local.Localize(Currency.Ien.ToString(), 5))));
            }
            else
            {
                // проверяем есть ли у пользователя уже этот баннер
                var hasBanner = await _bannerService.CheckUserHasBanner((long) context.User.Id, banner.Id);

                if (hasBanner)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyBannerAlready.Parse(
                        banner.Rarity.Localize().ToLower(), banner.Name)));
                }
                else
                {
                    // отнимаем у пользователя валюту для оплаты баннера
                    await _inventoryService.RemoveItemFromUser(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), banner.Price);
                    // добавляем пользователю баннер
                    await _bannerService.AddBannerToUser((long) context.User.Id, banner.Id);

                    // подверждаем успешную покупку баннера
                    embed.WithDescription(IzumiReplyMessage.ShopBuyBannerSuccess.Parse(
                        banner.Rarity.Localize().ToLower(), banner.Name,
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), banner.Price,
                        _local.Localize(Currency.Ien.ToString(), banner.Price)));

                    await _discordEmbedService.SendEmbed(context.User, embed);
                    await Task.CompletedTask;
                }
            }
        }
    }
}
