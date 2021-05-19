using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.Options;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.ReferralService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Microsoft.Extensions.Options;

namespace Hinode.Izumi.Services.Commands.UserCommands.ReferralCommands
{
    [CommandCategory(CommandCategory.Referral)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class ReferralSetCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IReferralService _referralService;
        private readonly IUserService _userService;
        private readonly ILocalizationService _local;
        private readonly IOptions<DiscordOptions> _options;

        public ReferralSetCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IReferralService referralService, IUserService userService, ILocalizationService local,
            IOptions<DiscordOptions> options)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _referralService = referralService;
            _userService = userService;
            _local = local;
            _options = options;
        }

        [Command("пригласил"), Alias("пригласила", "invited")]
        [Summary("Указать пригласившего пользователя")]
        [CommandUsage("!пригласил Холли", "!пригласила Рыбка")]
        public async Task ReferralSetTask(
            [Summary("Игровое имя")] [Remainder] string referralName)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем пригласившего пользователя
            var tUser = await _userService.GetUser(referralName);
            // проверяем есть ли у пользователя реферрер
            var hasReferrer = await _referralService.CheckUserHasReferrer((long) Context.User.Id);

            // проверяем что пользователь не указал самого себя
            if ((long) Context.User.Id == tUser.Id)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ReferralSetYourself.Parse()));
            }
            // проверяем что пользователь не указал Изуми
            else if ((long) _options.Value.BotId == tUser.Id)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ReferralSetIzumi.Parse()));
            }
            // проверяем что пользователь не указывал пригласившего
            else if (hasReferrer)
            {
                // получаем пригласившего пользователя из базы
                var rUser = await _referralService.GetUserReferrer((long) Context.User.Id);
                await Task.FromException(new Exception(IzumiReplyMessage.ReferralSetAlready.Parse(
                    emotes.GetEmoteOrBlank(rUser.Title.Emote()), rUser.Title.Localize(), rUser.Name)));
            }
            else
            {
                // добавляем информацию о приглашении
                await _referralService.AddUserReferrer((long) Context.User.Id, tUser.Id);

                var embed = new EmbedBuilder()
                    // подтверждаем что информация о приглашении добавлена
                    .WithDescription(IzumiReplyMessage.ReferralSetSuccess.Parse(
                        emotes.GetEmoteOrBlank(tUser.Title.Emote()), tUser.Title.Localize(), tUser.Name))
                    // бонус реферальной системы
                    .AddField(IzumiReplyMessage.ReferralRewardFieldName.Parse(),
                        $"{emotes.GetEmoteOrBlank(Box.Capital.Emote())} {_local.Localize(Box.Capital.ToString())}");

                await _discordEmbedService.SendEmbed(Context.User, embed);
                await Task.CompletedTask;
            }
        }
    }
}
