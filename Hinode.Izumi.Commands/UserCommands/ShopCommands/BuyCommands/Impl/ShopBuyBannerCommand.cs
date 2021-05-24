using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BannerService.Commands;
using Hinode.Izumi.Services.GameServices.BannerService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuyBannerCommand : IShopBuyBannerCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopBuyBannerCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long bannerId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем желаемый баннер из сменяемого магазина
            var banner = await _mediator.Send(new GetDynamicShopBannerQuery(bannerId));
            // получаем валюту пользователя
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery((long) context.User.Id, Currency.Ien));

            var embed = new EmbedBuilder()
                // баннер магазина баннеров
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopBanner)));

            // проверяем хватит ли пользователю денег на оплату баннера
            if (userCurrency.Amount < banner.Price)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyNoCurrency.Parse(
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), _local.Localize(Currency.Ien.ToString(), 5))));
            }
            else
            {
                // проверяем есть ли у пользователя уже этот баннер
                var hasBanner = await _mediator.Send(new CheckUserHasBannerQuery((long) context.User.Id, banner.Id));

                if (hasBanner)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyBannerAlready.Parse(
                        banner.Rarity.Localize().ToLower(), banner.Name)));
                }
                else
                {
                    // отнимаем у пользователя валюту для оплаты баннера
                    await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), banner.Price));
                    // добавляем пользователю баннер
                    await _mediator.Send(new AddBannerToUserCommand((long) context.User.Id, banner.Id));

                    // подверждаем успешную покупку баннера
                    embed.WithDescription(IzumiReplyMessage.ShopBuyBannerSuccess.Parse(
                        banner.Rarity.Localize().ToLower(), banner.Name,
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), banner.Price,
                        _local.Localize(Currency.Ien.ToString(), banner.Price)));

                    await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                    await Task.CompletedTask;
                }
            }
        }
    }
}
