using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CardService;
using Hinode.Izumi.Services.RpgServices.TrainingService;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckAddCommand
{
    [InjectableService]
    public class UserDeckAddCommand : IUserDeckAddCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ICardService _cardService;
        private readonly ITrainingService _trainingService;

        public UserDeckAddCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ICardService cardService, ITrainingService trainingService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _cardService = cardService;
            _trainingService = trainingService;
        }

        public async Task Execute(SocketCommandContext context, long cardId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем карточку
            var card = await _cardService.GetCard(cardId);

            // проверяем есть ли у пользователя такая карточка
            await _cardService.GetUserCard((long) context.User.Id, cardId);
            // проверяем не добавлена ли такая карточка в его колоду
            var hasCardInDeck = await _cardService.CheckCardInUserDeck((long) context.User.Id, card.Id);

            if (hasCardInDeck)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserDeckAddAlreadyInDeck.Parse(
                    card.Rarity.Localize(), card.Name, emotes.GetEmoteOrBlank("CardDeck"))));
            }
            else
            {
                // получаем размер колоды пользователя
                var deckLength = await _cardService.GetUserDeckLength((long) context.User.Id);

                // если размер колоды достиг лимита - нельзя добавить в нее карточку
                if (deckLength >= 5)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.UserDeckAddLengthMoreThen5.Parse(
                        emotes.GetEmoteOrBlank("CardDeck"))));
                }
                else
                {
                    // добавляем карточку в колоду пользователя
                    await _cardService.AddCardToDeck((long) context.User.Id, card.Id);

                    var embed = new EmbedBuilder()
                        // подверждаем что добавление карточки в колоду прошло успешно
                        .WithDescription(IzumiReplyMessage.UserDeckAddSuccess.Parse(
                            card.Rarity.Localize(true), card.Name, emotes.GetEmoteOrBlank("CardDeck")));

                    await _discordEmbedService.SendEmbed(context.User, embed);
                    // проверяем нужно ли двинуть прогресс обучения пользователя
                    await _trainingService.CheckStep((long) context.User.Id, TrainingStep.AddCardToDeck);
                    await Task.CompletedTask;
                }
            }
        }
    }
}
