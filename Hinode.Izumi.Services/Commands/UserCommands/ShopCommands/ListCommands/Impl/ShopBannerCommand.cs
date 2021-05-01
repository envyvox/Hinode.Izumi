using System;
using System.Globalization;
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
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.ListCommands.Impl
{
    [InjectableService]
    public class ShopBannerCommand : IShopBannerCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IBannerService _bannerService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _local;

        public ShopBannerCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IBannerService bannerService, IImageService imageService, ILocalizationService local)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _bannerService = bannerService;
            _imageService = imageService;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем баннеры из сменяемого магазина
            var banners = await _bannerService.GetDynamicShopBanner();

            var embed = new EmbedBuilder()
                // баннер магазина баннеров
                .WithImageUrl(await _imageService.GetImageUrl(Image.ShopBanner))
                .WithDescription(
                    // рассказываем как покупать баннеры
                    IzumiReplyMessage.ShopBannerDesc.Parse() +
                    // рассказываем о том, что товары меняются каждый день
                    IzumiReplyMessage.DynamicShopDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // выводим сколько осталось времени до смены товаров
                .WithFooter(IzumiReplyMessage.DynamicShopFooter.Parse(
                    (DateTime.Now - DateTime.Today.AddDays(1)).Humanize(2, new CultureInfo("ru-RU"))));

            // создаем embed field с информацией о каждом баннере
            foreach (var banner in banners)
            {
                embed.AddField(
                    $"{emotes.GetEmoteOrBlank("List")} `{banner.Id}` {banner.Rarity.Localize()} «{banner.Name}»",
                    IzumiReplyMessage.ShopBannerFieldDesc.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), banner.Price,
                        _local.Localize(Currency.Ien.ToString(), banner.Price), banner.Url));
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
