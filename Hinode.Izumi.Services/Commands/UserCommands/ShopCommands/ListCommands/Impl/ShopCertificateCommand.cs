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
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.ListCommands.Impl
{
    [InjectableService]
    public class ShopCertificateCommand : IShopCertificateCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly ICertificateService _certificateService;
        private readonly IImageService _imageService;

        public ShopCertificateCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocalizationService local, ICertificateService certificateService, IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _local = local;
            _certificateService = certificateService;
            _imageService = imageService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем сертификаты из базы
            var certificates = await _certificateService.GetAllCertificates();

            var embed = new EmbedBuilder()
                // рассказываем как покупать сертификаты
                .WithDescription(
                    IzumiReplyMessage.CapitalCertShopDesc.Parse()
                    + $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // баннер магазина сертификатов
                .WithImageUrl(await _imageService.GetImageUrl(Image.ShopCertificate));

            // создаем embed field для каждого сертификата
            foreach (var certificate in certificates)
            {
                embed.AddField(
                    $"{emotes.GetEmoteOrBlank("List")} `{certificate.Id}` {emotes.GetEmoteOrBlank("Certificate")} {certificate.Name} стоимостью {emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {certificate.Price} {_local.Localize(Currency.Ien.ToString(), certificate.Price)}",
                    certificate.Description + $"\n{emotes.GetEmoteOrBlank("Blank")}");
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
