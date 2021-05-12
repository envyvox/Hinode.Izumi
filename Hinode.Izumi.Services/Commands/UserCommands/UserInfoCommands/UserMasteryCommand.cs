using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.MasteryService;
using Hinode.Izumi.Services.RpgServices.ReputationService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserMasteryCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IImageService _imageService;
        private readonly IMasteryService _masteryService;
        private readonly IReputationService _reputationService;

        public UserMasteryCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IImageService imageService, IMasteryService masteryService, IReputationService reputationService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _imageService = imageService;
            _masteryService = masteryService;
            _reputationService = reputationService;
        }

        [Command("мастерство"), Alias("mastery")]
        [Summary("Посмотреть информацию о своем мастерстве")]
        public async Task UserMasteryTask()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем мастерство пользователя
            var userMasteries = await _masteryService.GetUserMastery((long) Context.User.Id);
            // получаем репутации пользователя
            var userReputations = await _reputationService.GetUserReputation((long) Context.User.Id);
            // получаем массив доступных репутаций
            var reputations = Enum.GetValues(typeof(Reputation)).Cast<Reputation>().ToArray();
            // получаем среднее значение репутаций пользователя
            var userAverageReputation =
                reputations.Sum(reputation =>
                    userReputations.ContainsKey(reputation) ? userReputations[reputation].Amount : 0) /
                reputations.Length;
            // считаем максимальное мастерство пользователя
            var userMaxMastery = ReputationStatusHelper
                .GetReputationStatus(userAverageReputation)
                .MaxMastery();

            var embed = new EmbedBuilder()
                // рассказываем про лимит мастерства
                .WithDescription(
                    IzumiReplyMessage.UserMasteryDesc.Parse(userMaxMastery) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // баннер мастерства
                .WithImageUrl(await _imageService.GetImageUrl(Image.Mastery));

            // создаем embed field для каждого мастерства
            foreach (var mastery in Enum.GetValues(typeof(Mastery))
                .Cast<Mastery>())
            {
                // получаем округленное количество мастерства пользователя
                var userMasteryAmount = Math.Round(
                    userMasteries.ContainsKey(mastery)
                        ? userMasteries[mastery].Amount
                        : 0,
                    // округление в меньшую сторону до двух чисел после точки
                    2, MidpointRounding.ToZero);

                embed.AddField(
                    IzumiReplyMessage.UserMasteryFieldName.Parse(
                        emotes.GetEmoteOrBlank(mastery.ToString()), userMasteryAmount, mastery.Localize()),
                    mastery.Description());
            }

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
