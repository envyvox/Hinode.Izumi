using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketInfoCommand
{
    [InjectableService]
    public class MarketInfoCommand : IMarketInfoCommand
    {
        private readonly IMediator _mediator;

        public MarketInfoCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var embed = new EmbedBuilder()
                // баннер рынка
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.LocationCapitalMarket)))
                .WithDescription(IzumiReplyMessage.MarketInfoDesc.Parse())
                // рассказываем как покупать на рынке
                .AddField(IzumiReplyMessage.MarketInfoBuyFieldName.Parse(),
                    IzumiReplyMessage.MarketInfoBuyFieldDesc.Parse())
                // рассказываем как продавать на рынке
                .AddField(IzumiReplyMessage.MarketInfoSellFieldName.Parse(),
                    IzumiReplyMessage.MarketInfoSellFieldDesc.Parse())
                // рассказываем как управлять своими заявками на рынке
                .AddField(IzumiReplyMessage.MarketInfoRequestFieldName.Parse(),
                    IzumiReplyMessage.MarketInfoRequestFieldDesc.Parse())
                // выводим список разрешенных групп товаров на рынке
                .AddField(IzumiReplyMessage.MarketInfoGroupsFieldName.Parse(),
                    Enum.GetValues(typeof(MarketCategory))
                        .Cast<MarketCategory>()
                        .Aggregate(string.Empty, (current, category) =>
                            current +
                            $"{emotes.GetEmoteOrBlank("List")} `{category.GetHashCode()}` {category.Localize()}\n"));

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));

            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _mediator.Send(new CheckUserTutorialStepCommand(
                (long) context.User.Id, TutorialStep.CheckCapitalMarket));

            await Task.CompletedTask;
        }
    }
}
