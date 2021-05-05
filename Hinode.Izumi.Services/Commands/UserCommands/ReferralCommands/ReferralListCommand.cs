using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.ReferralService;

namespace Hinode.Izumi.Services.Commands.UserCommands.ReferralCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class ReferralListCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IReferralService _referralService;
        private readonly ILocalizationService _local;

        public ReferralListCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IReferralService referralService, ILocalizationService local)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _referralService = referralService;
            _local = local;
        }

        [Command("приглашения"), Alias("referrals")]
        public async Task ReferralListTask()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // проверяем есть ли у пользователя реферрер
            var hasReferrer = await _referralService.CheckUserHasReferrer((long) Context.User.Id);
            // получаем рефералов пользователя
            var userReferrals = await _referralService.GetUserReferrals((long) Context.User.Id);

            // заполняем строку реферрера в зависимости от того, есть ли он у пользователя
            string referrerString;
            // если есть - выводим информацию о нем
            if (hasReferrer)
            {
                // получаем реферерра пользователя
                var referrer = await _referralService.GetUserReferrer((long) Context.User.Id);
                referrerString = IzumiReplyMessage.ReferralListReferrerFieldDesc.Parse(
                    emotes.GetEmoteOrBlank(referrer.Title.Emote()), referrer.Title.Localize(), referrer.Name,
                    emotes.GetEmoteOrBlank(Box.Capital.Emote()), _local.Localize(Box.Capital.ToString()));
            }
            // если нет - предлагаем указать
            else
            {
                referrerString = IzumiReplyMessage.ReferralListReferrerNull.Parse(
                    emotes.GetEmoteOrBlank(Box.Capital.Emote()), _local.Localize(Box.Capital.ToString()));
            }

            // заполняем информацию о рефералах пользователя
            var referralString = userReferrals.Aggregate(string.Empty, (current, user) => current +
                $"{emotes.GetEmoteOrBlank("List")} {emotes.GetEmoteOrBlank(user.Title.Emote())} {user.Title.Localize()} **{user.Name}**\n");

            var embed = new EmbedBuilder()
                .WithDescription(
                    IzumiReplyMessage.ReferralListDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // реферер
                .AddField(IzumiReplyMessage.ReferralListReferrerFieldName.Parse(),
                    referrerString +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                .AddField(IzumiReplyMessage.ReferralListRewardsFieldName.Parse(),
                    IzumiReplyMessage.ReferralListRewardsFieldDesc.Parse(
                        emotes.GetEmoteOrBlank(Box.Capital.Emote()), _local.Localize(Box.Capital.ToString()),
                        2, _local.Localize(Box.Capital.ToString(), 2), 3,
                        emotes.GetEmoteOrBlank(Currency.Pearl.ToString()), 10,
                        _local.Localize(Currency.Pearl.ToString(), 10),
                        emotes.GetEmoteOrBlank(Title.Yatagarasu.Emote()), Title.Yatagarasu.Localize(), 15,
                        emotes.GetEmoteOrBlank(userReferrals.Length >= 2 ? "Checkmark" : "List"),
                        emotes.GetEmoteOrBlank(userReferrals.Length >= 4 ? "Checkmark" : "List"),
                        emotes.GetEmoteOrBlank(userReferrals.Length >= 5 ? "Checkmark" : "List"),
                        emotes.GetEmoteOrBlank(userReferrals.Length >= 9 ? "Checkmark" : "List"),
                        emotes.GetEmoteOrBlank(userReferrals.Length >= 10 ? "Checkmark" : "List"),
                        emotes.GetEmoteOrBlank("List")))
                // приглашенные пользователи
                .AddField(IzumiReplyMessage.ReferralListReferralsFieldName.Parse(),
                    referralString.Length > 0
                        // если у пользователя есть приглашенные пользователи - тогда нужно вывести информацию о них
                        ? referralString.Length > 1024
                            // если у пользователя много приглашенных пользователей - тогда вывести их всех не получится
                            // поэтому отображаем только количество
                            ? IzumiReplyMessage.ReferralListReferralsOutOfLimit.Parse(userReferrals.Length)
                            // если длительности строки хватает чтобы их отобразить - тогда выводим информацию о них
                            : referralString
                        // если нет - выводим отдельную строку
                        : IzumiReplyMessage.ReferralListReferralsNull.Parse(
                            emotes.GetEmoteOrBlank(Box.Capital.Emote()),
                            emotes.GetEmoteOrBlank(Currency.Pearl.ToString())));

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
