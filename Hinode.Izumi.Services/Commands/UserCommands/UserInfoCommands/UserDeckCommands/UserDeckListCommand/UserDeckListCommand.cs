using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CardService;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckListCommand
{
    [InjectableService]
    public class UserDeckListCommand : IUserDeckListCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ICardService _cardService;

        public UserDeckListCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ICardService cardService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _cardService = cardService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем колоду пользователя
            var userDeck = await _cardService.GetUserDeck((long) context.User.Id);

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

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
