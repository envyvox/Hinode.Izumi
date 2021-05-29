using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CardService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckListCommand
{
    [InjectableService]
    public class UserDeckListCommand : IUserDeckListCommand
    {
        private readonly IMediator _mediator;

        public UserDeckListCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем колоду пользователя
            var userDeck = await _mediator.Send(new GetUserDeckQuery((long) context.User.Id));

            var embed = new EmbedBuilder()
                // рассказываем о командах управления колодой
                .WithDescription(IzumiReplyMessage.UserDeckListDesc.Parse(
                    emotes.GetEmoteOrBlank("CardDeck")))
                // показываем количество карточек в колоде
                .WithFooter(IzumiReplyMessage.UserDeckListFooter.Parse(
                    userDeck.Length));

            // если в колоде пользователя нет карточек - предлагаем ему их добавить
            if (userDeck.Length < 1)
            {
                embed.AddField(IzumiReplyMessage.UserDeckListLengthLessThen1FieldName.Parse(),
                    IzumiReplyMessage.UserDeckListLengthLessThen1FieldDesc.Parse(
                        emotes.GetEmoteOrBlank("CardDeck")));
            }
            else
            {
                // создаем embed field для каждой карточки в колоде
                foreach (var card in userDeck)
                {
                    embed.AddField(
                        $"{emotes.GetEmoteOrBlank("List")} `{card.Id}` {card.Rarity.Localize()} «{card.Name}»",
                        IzumiReplyMessage.CardDetailedDesc.Parse(
                            card.Anime, card.Effect.Localize(), card.Url));
                }
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
