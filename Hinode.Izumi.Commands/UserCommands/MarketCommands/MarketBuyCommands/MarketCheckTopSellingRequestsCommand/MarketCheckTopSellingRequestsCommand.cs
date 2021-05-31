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
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.MarketService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketBuyCommands.
    MarketCheckTopSellingRequestsCommand
{
    [InjectableService]
    public class MarketCheckTopSellingRequestsCommand : IMarketCheckTopSellingRequestsCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public MarketCheckTopSellingRequestsCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, MarketCategory category, string pattern = null)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем топ 5 заявок в этой категории
            var requests = await _mediator.Send(new GetMarketRequestByParamsQuery(category, true, pattern));

            var embed = new EmbedBuilder()
                // баннер рынка
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.LocationCapitalMarket)))
                // рассказываем как покупать на рынке
                .WithDescription(
                    IzumiReplyMessage.MarketBuyDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            // для каждой заявки создаем embed field
            foreach (var request in requests)
            {
                // получаем вледельца заявки
                var user = await _mediator.Send(new GetUserByIdQuery(request.UserId));
                // получаем название товара
                var itemName = await _mediator.Send(new GetItemNameQuery(category switch
                {
                    MarketCategory.Gathering => InventoryCategory.Gathering,
                    MarketCategory.Crafting => InventoryCategory.Crafting,
                    MarketCategory.Alcohol => InventoryCategory.Alcohol,
                    MarketCategory.Drink => InventoryCategory.Drink,
                    MarketCategory.Food => InventoryCategory.Food,
                    MarketCategory.Crop => InventoryCategory.Crop,
                    _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
                }, request.ItemId));

                // выводим информацию о заявке
                embed.AddField(IzumiReplyMessage.MarketBuyFieldName.Parse(
                        emotes.GetEmoteOrBlank("List"), request.Id, emotes.GetEmoteOrBlank(user.Title.Emote()),
                        user.Title.Localize(), user.Name, emotes.GetEmoteOrBlank(itemName),
                        _local.Localize(request.Category, request.ItemId)),
                    IzumiReplyMessage.MarketRequestInfo.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), request.Price,
                        _local.Localize(Currency.Ien.ToString(), request.Price), request.Amount) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");
            }

            if (embed.Fields.Count < 1)
            {
                embed.AddField(IzumiReplyMessage.MarketRequestListNullFieldName.Parse(
                        emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.MarketRequestListNullFieldDesc.Parse());
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
