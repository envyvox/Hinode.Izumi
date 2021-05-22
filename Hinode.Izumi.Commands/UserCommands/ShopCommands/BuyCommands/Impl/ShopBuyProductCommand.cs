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
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.ProductService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuyProductCommand : IShopBuyProductCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopBuyProductCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long productId, long amount)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем продукт
            var product = await _mediator.Send(new GetProductQuery(productId));
            // получаем валюту пользователя
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery((long) context.User.Id, Currency.Ien));

            // проверяем хватит ли пользователю денег на оплату продуктов
            if (userCurrency.Amount < product.Price * amount)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyNoCurrency.Parse(
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), _local.Localize(Currency.Ien.ToString(), 5))));
            }
            else
            {
                // забираем у пользователя деньги на оплату продуктов
                await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                    (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                    product.Price * amount));
                // добавляем пользователю продукты
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    (long) context.User.Id, InventoryCategory.Product, product.Id, amount));

                var embed = new EmbedBuilder()
                    // баннер магазина продуктов
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopProduct)))
                    // подверждаем успешную покупку продуктов
                    .WithDescription(IzumiReplyMessage.ProductShopBuySuccess.Parse(
                        emotes.GetEmoteOrBlank(product.Name), amount, _local.Localize(product.Name, amount),
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), product.Price * amount,
                        _local.Localize(Currency.Ien.ToString(), product.Price * amount)));

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                await Task.CompletedTask;
            }
        }
    }
}
