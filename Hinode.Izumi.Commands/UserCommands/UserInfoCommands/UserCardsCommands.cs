using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CardService.Queries;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserCardsCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public UserCardsCommands(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("карточки"), Alias("cards")]
        [Summary("Посмотреть свою коллекцию карточек")]
        public async Task UserCardsTask()
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем карточки пользователя
            var userCards = await _mediator.Send(new GetUserCardsQuery((long) Context.User.Id));
            // получаем количество всех карточек в базе
            var allCardLength = await _mediator.Send(new GetAllCardLengthQuery());

            var embed = new EmbedBuilder()
                // рассказываем о доступных командах карточек
                .WithDescription(IzumiReplyMessage.UserCardListDesc.Parse())
                // показываем сколько карточек у пользователя собрано из общего количества
                .WithFooter(IzumiReplyMessage.UserCardListFooter.Parse(
                    userCards.Length, allCardLength));

            switch (userCards.Length)
            {
                // если у пользователя нет карточек, рассказываем где взять первую
                case < 1:
                    embed.AddField(IzumiReplyMessage.UserCardListLengthLessThen1FieldName.Parse(),
                        IzumiReplyMessage.UserCardListLengthLessThen1FieldDesc.Parse(
                            Location.Capital.Localize(true)));
                    break;

                // если у пользователя больше 15 карточек, то их нельзя вывести в дискорде
                case > 15:
                    // TODO тут нужно будет дать пользователю ссылку на сайт, где он сможет посмотреть свои карточки
                    embed.AddField(IzumiReplyMessage.UserCardListLengthMoreThen15FieldName.Parse(),
                        IzumiReplyMessage.UserCardListLengthMoreThen15FieldDesc.Parse());
                    break;

                default:
                {
                    // создаем embed field для каждой карточки пользователя
                    foreach (var card in userCards)
                    {
                        embed.AddField(
                            $"{emotes.GetEmoteOrBlank("List")} `{card.Id}` {card.Rarity.Localize()} «{card.Name}»",
                            IzumiReplyMessage.CardDetailedDesc.Parse(
                                card.Anime, card.Effect.Localize(), card.Url));
                    }

                    break;
                }
            }

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _mediator.Send(new CheckUserTutorialStepCommand((long) Context.User.Id, TutorialStep.CheckCards));
            await Task.CompletedTask;
        }
    }
}
