using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CertificateService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.ListCommands.Impl
{
    [InjectableService]
    public class ShopCertificateCommand : IShopCertificateCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopCertificateCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем сертификаты из базы
            var certificates = await _mediator.Send(new GetAllCertificatesQuery());

            var embed = new EmbedBuilder()
                // рассказываем как покупать сертификаты
                .WithDescription(
                    IzumiReplyMessage.CapitalCertShopDesc.Parse()
                    + $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // баннер магазина сертификатов
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopCertificate)));

            // создаем embed field для каждого сертификата
            foreach (var certificate in certificates)
            {
                embed.AddField(
                    $"{emotes.GetEmoteOrBlank("List")} `{certificate.Id}` {emotes.GetEmoteOrBlank("Certificate")} {certificate.Name} стоимостью {emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {certificate.Price} {_local.Localize(Currency.Ien.ToString(), certificate.Price)}",
                    certificate.Description + $"\n{emotes.GetEmoteOrBlank("Blank")}");
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
