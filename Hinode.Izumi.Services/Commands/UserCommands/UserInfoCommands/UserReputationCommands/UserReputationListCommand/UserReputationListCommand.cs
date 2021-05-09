using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.ReputationService;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserReputationCommands.UserReputationListCommand
{
    [InjectableService]
    public class UserReputationListCommand : IUserReputationListCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IReputationService _reputationService;

        public UserReputationListCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IReputationService reputationService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _reputationService = reputationService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем репутации пользователя
            var userReputations = await _reputationService.GetUserReputation((long) context.User.Id);
            // получаем массив доступных репутаций
            var reputations = Enum.GetValues(typeof(Reputation)).Cast<Reputation>().ToArray();
            // получаем среднее значение репутаций пользователя
            var userAverageReputation =
                reputations.Sum(reputation =>
                    userReputations.ContainsKey(reputation) ? userReputations[reputation].Amount : 0) /
                reputations.Length;
            // определяем репутационный статус по среднему значению репутаций пользователя
            var userReputationStatus = ReputationStatusHelper.GetReputationStatus(userAverageReputation);

            var embed = new EmbedBuilder()
                // репутационный рейтинг и как смотреть информацию о репутации
                .WithDescription(
                    IzumiReplyMessage.UserReputationListDesc.Parse(
                        userReputationStatus.Localize(),
                        emotes.GetEmoteOrBlank(ReputationStatusHelper.Emote(userAverageReputation)),
                        userAverageReputation) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // репутация пользователя
                .AddField(IzumiReplyMessage.UserReputationListFieldName.Parse(),
                    Enum.GetValues(typeof(Reputation))
                        .Cast<Reputation>()
                        .Aggregate(string.Empty, (current, reputation) =>
                            current +
                            $"{emotes.GetEmoteOrBlank("List")} `{reputation.GetHashCode()}` {emotes.GetEmoteOrBlank(reputation.Emote(userReputations.ContainsKey(reputation) ? userReputations[reputation].Amount : 0))} {(userReputations.ContainsKey(reputation) ? $"{userReputations[reputation].Amount}" : "0")} в **{reputation.Location().Localize(true)}**\n"))
                .WithFooter(IzumiReplyMessage.UserReputationListFooter.Parse());

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
