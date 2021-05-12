using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CardService;
using Hinode.Izumi.Services.RpgServices.TrainingService;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserCardsCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ICardService _cardService;
        private readonly ITrainingService _trainingService;

        public UserCardsCommands(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ICardService cardService, ITrainingService trainingService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _cardService = cardService;
            _trainingService = trainingService;
        }

        [Command("карточки"), Alias("cards")]
        [Summary("Посмотреть свою коллекцию карточек")]
        public async Task UserCardsTask()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем карточки пользователя
            var userCards = await _cardService.GetUserCard((long) Context.User.Id);
            // получаем количество всех карточек в базе
            var allCardLength = await _cardService.GetAllCardLength();

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

            await _discordEmbedService.SendEmbed(Context.User, embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _trainingService.CheckStep((long) Context.User.Id, TrainingStep.CheckCards);
            await Task.CompletedTask;
        }
    }
}
