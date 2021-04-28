using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CardService;

namespace Hinode.Izumi.Services.Commands.UserCommands.WorldInfoCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class CardInfoCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly ICardService _cardService;
        private readonly IEmoteService _emoteService;

        public CardInfoCommand(IDiscordEmbedService discordEmbedService, ICardService cardService,
            IEmoteService emoteService)
        {
            _discordEmbedService = discordEmbedService;
            _cardService = cardService;
            _emoteService = emoteService;
        }

        [Command("карточка"), Alias("card")]
        public async Task CardInfoTask(long cardId)
        {
            // получаем карточку с таким id
            var card = await _cardService.GetCard(cardId);
            // получаем все иконки из базы
            var emotes = await _emoteService.GetEmotes();

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

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
