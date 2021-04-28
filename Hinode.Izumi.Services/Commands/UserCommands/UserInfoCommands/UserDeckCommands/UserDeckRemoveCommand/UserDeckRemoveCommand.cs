using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CardService;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckRemoveCommand
{
    [InjectableService]
    public class UserDeckRemoveCommand : IUserDeckRemoveCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ICardService _cardService;

        public UserDeckRemoveCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ICardService cardService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _cardService = cardService;
        }

        public async Task Execute(SocketCommandContext context, long cardId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем карточку
            var card = await _cardService.GetCard(cardId);

            // проверяем есть ли такая карточк у пользователя
            await _cardService.GetUserCard((long) context.User.Id, cardId);
            // проверяем есть ли такая карточка в кололе пользователя
            var hasCardInDeck = await _cardService.CheckCardInUserDeck((long) context.User.Id, card.Id);

            // если нет - ее нельзя убрать из колоды
            if (!hasCardInDeck)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserDeckRemoveNotInDeck.Parse(
                    card.Rarity.Localize(), card.Name, emotes.GetEmoteOrBlank("CardDeck"))));
            }
            else
            {
                // убираем карточку из колоды пользователя
                await _cardService.RemoveCardFromDeck((long) context.User.Id, card.Id);

                var embed = new EmbedBuilder()
                    // подверждаем что убирание карточки из колоды прошло успешно
                    .WithDescription(IzumiReplyMessage.UserDeckRemoveSuccess.Parse(
                        card.Rarity.Localize(true), card.Name, emotes.GetEmoteOrBlank("CardDeck")));

                await _discordEmbedService.SendEmbed(context.User, embed);
                await Task.CompletedTask;
            }
        }
    }
}
