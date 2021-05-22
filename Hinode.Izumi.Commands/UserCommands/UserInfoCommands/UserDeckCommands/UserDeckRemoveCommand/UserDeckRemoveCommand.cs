using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CardService.Commands;
using Hinode.Izumi.Services.GameServices.CardService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckRemoveCommand
{
    [InjectableService]
    public class UserDeckRemoveCommand : IUserDeckRemoveCommand
    {
        private readonly IMediator _mediator;

        public UserDeckRemoveCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context, long cardId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем карточку
            var card = await _mediator.Send(new GetCardQuery(cardId));

            // проверяем есть ли такая карточк у пользователя
            await _mediator.Send(new GetUserCardQuery((long) context.User.Id, cardId));
            // проверяем есть ли такая карточка в кололе пользователя
            var hasCardInDeck = await _mediator.Send(new CheckCardInUserDeckQuery((long) context.User.Id, card.Id));

            // если нет - ее нельзя убрать из колоды
            if (!hasCardInDeck)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserDeckRemoveNotInDeck.Parse(
                    card.Rarity.Localize(), card.Name, emotes.GetEmoteOrBlank("CardDeck"))));
            }
            else
            {
                // убираем карточку из колоды пользователя
                await _mediator.Send(new RemoveCardFromDeckCommand((long) context.User.Id, card.Id));

                var embed = new EmbedBuilder()
                    // подверждаем что убирание карточки из колоды прошло успешно
                    .WithDescription(IzumiReplyMessage.UserDeckRemoveSuccess.Parse(
                        card.Rarity.Localize(true), card.Name, emotes.GetEmoteOrBlank("CardDeck")));

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                await Task.CompletedTask;
            }
        }
    }
}
