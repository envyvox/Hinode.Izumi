using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CertificateService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuyCertificateCommand : IShopBuyCertificateCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ICertificateService _certificateService;
        private readonly IInventoryService _inventoryService;
        private readonly ILocalizationService _local;
        private readonly IImageService _imageService;

        public ShopBuyCertificateCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ICertificateService certificateService, IInventoryService inventoryService, ILocalizationService local,
            IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _certificateService = certificateService;
            _inventoryService = inventoryService;
            _local = local;
            _imageService = imageService;
        }

        public async Task Execute(SocketCommandContext context, long certificateId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем сертификат
            var certificate = await _certificateService.GetCertificate(certificateId);
            // проверяем есть ли у пользователя уже такой сертификат
            var hasCert = await _certificateService.CheckUserCertificate((long) context.User.Id, certificate.Id);

            if (hasCert)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyCertAlready.Parse(
                    emotes.GetEmoteOrBlank("Certificate"), certificate.Name)));
            }
            else
            {
                // получаем валюту пользователя
                var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id, Currency.Ien);

                // проверяем хватит ли пользователю денег на оплату сертификата
                if (userCurrency.Amount < certificate.Price)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyNoCurrency.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), _local.Localize(Currency.Ien.ToString(), 5))));
                }
                else
                {
                    // отнимаем у пользователя деньги на оплату сертификата
                    await _inventoryService.RemoveItemFromUser(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                        certificate.Price);
                    // добавляем пользователю сертификат
                    await _certificateService.AddCertificateToUser((long) context.User.Id, certificate.Id);

                    var embed = new EmbedBuilder()
                        // баннер магазина сертификатов
                        .WithImageUrl(await _imageService.GetImageUrl(Image.ShopCertificate))
                        // подверждаем успешную покупку сертификата
                        .WithDescription(IzumiReplyMessage.ShopBuyCertSuccess.Parse(
                            emotes.GetEmoteOrBlank("Certificate"), certificate.Name));

                    await _discordEmbedService.SendEmbed(context.User, embed);
                    await Task.CompletedTask;
                }
            }
        }
    }
}
