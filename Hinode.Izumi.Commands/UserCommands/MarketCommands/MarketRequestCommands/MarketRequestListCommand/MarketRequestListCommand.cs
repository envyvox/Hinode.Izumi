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
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.CraftingService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.MarketService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketRequestCommands.MarketRequestListCommand
{
    [InjectableService]
    public class MarketRequestListCommand : IMarketRequestListCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public MarketRequestListCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем заявки пользователя на рынке
            var requests = await _mediator.Send(new GetMarketUserRequestsQuery((long) context.User.Id));

            var embed = new EmbedBuilder()
                // баннер рынка
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.LocationCapitalMarket)))
                // рассказываем как отменять заявки
                .WithDescription(
                    IzumiReplyMessage.MarketRequestListDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            // для каждой заявки создаем embed field
            foreach (var request in requests)
            {
                // получаем название товара
                var itemName = await _mediator.Send(new GetItemNameQuery(
                    request.Category switch
                    {
                        MarketCategory.Gathering => InventoryCategory.Gathering,
                        MarketCategory.Crafting => InventoryCategory.Crafting,
                        MarketCategory.Alcohol => InventoryCategory.Alcohol,
                        MarketCategory.Drink => InventoryCategory.Drink,
                        MarketCategory.Food => InventoryCategory.Food,
                        _ => throw new ArgumentOutOfRangeException()
                    }, request.ItemId));

                // выводим информацию о заявке
                embed.AddField(IzumiReplyMessage.MarketUserRequestFieldName.Parse(
                        emotes.GetEmoteOrBlank("List"), request.Id, request.Selling ? "Продаете" : "Покупаете",
                        emotes.GetEmoteOrBlank(itemName), _local.Localize(request.Category, request.ItemId)),
                    IzumiReplyMessage.MarketRequestInfo.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), request.Price,
                        _local.Localize(Currency.Ien.ToString(), request.Price), request.Amount) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");
            }

            if (embed.Fields.Count < 1)
            {
                embed.AddField(IzumiReplyMessage.MarketUserRequestListNullFieldName.Parse(),
                    IzumiReplyMessage.MarketUserRequestListNullFieldDesc.Parse());
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
