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
using Hinode.Izumi.Services.GameServices.MarketService.Commands;
using Hinode.Izumi.Services.GameServices.MarketService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketRequestCommands.MarketRequestCancelCommand
{
    [InjectableService]
    public class MarketRequestCancelCommand : IMarketRequestCancelCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public MarketRequestCancelCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long requestId)
        {
            // получаем заявку
            var request = await _mediator.Send(new GetMarketRequestByIdQuery(requestId));

            // проверяем что эта заявка принадлежит пользователю
            if ((long) context.User.Id != request.UserId)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.MarketRequestWrongUser.Parse()));
            }
            else
            {
                // получаем иконки из базы
                var emotes = await _mediator.Send(new GetEmotesQuery());
                // получаем название товара
                var itemName = await _mediator.Send(new GetItemNameQuery(
                    request.Category switch
                    {
                        MarketCategory.Gathering => InventoryCategory.Gathering,
                        MarketCategory.Crafting => InventoryCategory.Crafting,
                        MarketCategory.Alcohol => InventoryCategory.Alcohol,
                        MarketCategory.Drink => InventoryCategory.Drink,
                        MarketCategory.Food => InventoryCategory.Food,
                        MarketCategory.Crop => InventoryCategory.Crop,
                        _ => throw new ArgumentOutOfRangeException()
                    }, request.ItemId));

                var embed = new EmbedBuilder()
                    // баннер рынка
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.LocationCapitalMarket)));

                // если эта заявка была на продажу
                if (request.Selling)
                {
                    // возвращаем пользователю предметы, которые он выставлял на продажу
                    await _mediator.Send(new AddItemToUserByMarketCategoryCommand(
                        (long) context.User.Id, request.Category, request.ItemId, request.Amount));

                    // подверждаем успешное удаление заявки
                    embed.WithDescription(IzumiReplyMessage.MarketRequestSellCancel.Parse(
                        emotes.GetEmoteOrBlank(itemName), request.Amount,
                        _local.Localize(request.Category, request.ItemId, request.Amount)));
                }
                // если эта заявка была на покупку
                else
                {
                    // возвращаем пользователю деньги, которые он выставил на покупку предметов
                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                        request.Price * request.Amount));

                    // подверждаем успешное удаление заявки
                    embed.WithDescription(IzumiReplyMessage.MarketRequestBuyCancel.Parse(
                        emotes.GetEmoteOrBlank(itemName), request.Amount,
                        _local.Localize(request.Category, request.ItemId, request.Amount),
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), request.Price * request.Amount,
                        _local.Localize(Currency.Ien.ToString(),
                            request.Price * request.Amount)));
                }

                // удаляем заявку с рынка
                await _mediator.Send(new DeleteMarketRequestCommand(requestId));
                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                await Task.CompletedTask;
            }
        }
    }
}
