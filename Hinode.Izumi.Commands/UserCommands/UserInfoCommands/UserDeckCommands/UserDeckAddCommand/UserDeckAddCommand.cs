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
using Hinode.Izumi.Services.GameServices.CardService.Commands;
using Hinode.Izumi.Services.GameServices.CardService.Queries;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckAddCommand
{
    [InjectableService]
    public class UserDeckAddCommand : IUserDeckAddCommand
    {
        private readonly IMediator _mediator;

        public UserDeckAddCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context, long cardId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем карточку
            var card = await _mediator.Send(new GetCardQuery(cardId));

            // проверяем есть ли у пользователя такая карточка
            await _mediator.Send(new GetUserCardQuery((long) context.User.Id, cardId));
            // проверяем не добавлена ли такая карточка в его колоду
            var hasCardInDeck = await _mediator.Send(new CheckCardInUserDeckQuery((long) context.User.Id, card.Id));

            if (hasCardInDeck)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserDeckAddAlreadyInDeck.Parse(
                    card.Rarity.Localize(), card.Name, emotes.GetEmoteOrBlank("CardDeck"))));
            }
            else
            {
                // получаем размер колоды пользователя
                var deckLength = await _mediator.Send(new GetUserDeckLengthQuery((long) context.User.Id));

                // если размер колоды достиг лимита - нельзя добавить в нее карточку
                if (deckLength >= 5)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.UserDeckAddLengthMoreThen5.Parse(
                        emotes.GetEmoteOrBlank("CardDeck"))));
                }
                else
                {
                    // добавляем карточку в колоду пользователя
                    await _mediator.Send(new AddCardToDeckCommand((long) context.User.Id, card.Id));

                    var embed = new EmbedBuilder()
                        // подверждаем что добавление карточки в колоду прошло успешно
                        .WithDescription(IzumiReplyMessage.UserDeckAddSuccess.Parse(
                            card.Rarity.Localize(true), card.Name, emotes.GetEmoteOrBlank("CardDeck")));

                    await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));

                    // проверяем нужно ли двинуть прогресс обучения пользователя
                    await _mediator.Send(new CheckUserTutorialStepCommand(
                        (long) context.User.Id, TutorialStep.AddCardToDeck));

                    await Task.CompletedTask;
                }
            }
        }
    }
}
