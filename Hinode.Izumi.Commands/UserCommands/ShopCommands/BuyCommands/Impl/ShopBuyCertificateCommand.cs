using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CertificateService.Commands;
using Hinode.Izumi.Services.GameServices.CertificateService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuyCertificateCommand : IShopBuyCertificateCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopBuyCertificateCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long certificateId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем сертификат
            var certificate = await _mediator.Send(new GetCertificateQuery(certificateId));
            // проверяем есть ли у пользователя уже такой сертификат
            var hasCert = await _mediator.Send(new CheckUserHasCertificateQuery(
                (long) context.User.Id, certificate.Id));

            if (hasCert)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyCertAlready.Parse(
                    emotes.GetEmoteOrBlank("Certificate"), certificate.Name)));
            }
            else
            {
                // получаем валюту пользователя
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery((long) context.User.Id, Currency.Ien));

                // проверяем хватит ли пользователю денег на оплату сертификата
                if (userCurrency.Amount < certificate.Price)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyNoCurrency.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), _local.Localize(Currency.Ien.ToString(), 5))));
                }
                else
                {
                    // отнимаем у пользователя деньги на оплату сертификата
                    await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                        certificate.Price));
                    // добавляем пользователю сертификат
                    await _mediator.Send(new AddCertificateToUserCommand((long) context.User.Id, certificate.Id));

                    var embed = new EmbedBuilder()
                        // баннер магазина сертификатов
                        .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopCertificate)))
                        // подверждаем успешную покупку сертификата
                        .WithDescription(IzumiReplyMessage.ShopBuyCertSuccess.Parse(
                            emotes.GetEmoteOrBlank("Certificate"), certificate.Name));

                    await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                    await Task.CompletedTask;
                }
            }
        }
    }
}
