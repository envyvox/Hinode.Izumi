using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CardService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.WorldInfoCommands
{
    [CommandCategory(CommandCategory.Cards, CommandCategory.WorldInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class CardInfoCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public CardInfoCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("карточка"), Alias("card")]
        [Summary("Посмотреть информацию об указанной карточке")]
        [CommandUsage("!карточка 1", "!карточка 10")]
        public async Task CardInfoTask(
            [Summary("Номер карточки")] long cardId)
        {
            // получаем карточку с таким id
            var card = await _mediator.Send(new GetCardQuery(cardId));
            // получаем все иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());

            var embed = new EmbedBuilder()
                // Id карточки
                .AddField(IzumiReplyMessage.CardInfoIdFieldName.Parse(),
                    $"{emotes.GetEmoteOrBlank("List")} `{card.Id}`")
                // редкость карточки
                .AddField(IzumiReplyMessage.CardInfoRarityFieldName.Parse(),
                    card.Rarity.Localize())
                // название карточки
                .AddField(IzumiReplyMessage.CardInfoNameFieldName.Parse(),
                    card.Name)
                // название тайтла
                .AddField(IzumiReplyMessage.CardInfoAnimeFieldName.Parse(),
                    card.Anime)
                // изображение карточки
                .WithImageUrl(card.Url);

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
