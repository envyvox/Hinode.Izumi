using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.EmoteService.Models;
using Hinode.Izumi.Services.RpgServices.FamilyService;
using Hinode.Izumi.Services.RpgServices.FamilyService.Models;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteListCommand
{
    [InjectableService]
    public class FamilyInviteListCommand : IFamilyInviteListCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IFamilyService _familyService;
        private readonly IUserService _userService;

        private Dictionary<string, EmoteModel> _emotes;

        public FamilyInviteListCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IFamilyService familyService, IUserService userService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _familyService = familyService;
            _userService = userService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            _emotes = await _emoteService.GetEmotes();
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _familyService.CheckUserHasFamily((long) context.User.Id);

            var embed = new EmbedBuilder();

            if (!hasFamily)
            {
                // получаем отправленные ему приглашения
                var invites = await _familyService.GetUserFamilyInvites((long) context.User.Id);
                // получаем локализированную строку приглашений
                var inviteString = invites.Aggregate(string.Empty,
                    (current, invite) => current + DisplayUserFamilyInvite(invite).Result);

                embed
                    // рассказываем как принимать или отказываться от приглашений
                    .WithDescription(IzumiReplyMessage.FamilyInviteListFamilyNullDesc.Parse())
                    // выводим список приглашений
                    .AddField(IzumiReplyMessage.FamilyInviteListFieldName.Parse(),
                        inviteString.Length > 0
                            ? inviteString
                            // если у него нет приглашений - выводим отдельную строку
                            : IzumiReplyMessage.FamilyInviteListFieldDescNull.Parse());
            }
            else
            {
                var userFamily = await _familyService.GetUserFamily((long) context.User.Id);

                // если пользователь обычный член семьи, он не может просматривать отправленные семьей приглашения
                if (userFamily.Status == UserInFamilyStatus.Default)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyInviteListCantWatch.Parse()));
                }
                else
                {
                    // получаем отправленные семьей приглашения
                    var invites = await _familyService.GetFamilyInvites(userFamily.FamilyId);
                    // получаем локализированную строку приглашений
                    var inviteString = invites.Aggregate(string.Empty,
                        (current, invite) => current + DisplayFamilyInvite(invite).Result);

                    embed
                        // рассказываем как отправлять или отменять приглашения
                        .WithDescription(IzumiReplyMessage.FamilyInviteListFamilyNotNullDesc.Parse())
                        // выводит список отправленных приглашений
                        .AddField(IzumiReplyMessage.FamilyInviteListFieldName.Parse(),
                            inviteString.Length > 0
                                ? inviteString
                                // если семья не отправляла приглашений - выводим отдельную строку
                                : IzumiReplyMessage.FamilyInviteListFieldDescNull.Parse());
                }
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Возвращает локализированную строку с информацией о полученном приглашении в семью.
        /// </summary>
        /// <param name="invite">Приглашение.</param>
        /// <returns>Локализированная строка с информацией о полученном приглашении в семью.</returns>
        private async Task<string> DisplayUserFamilyInvite(FamilyInviteModel invite)
        {
            // получаем семью, которая отправила приглашение
            var family = await _familyService.GetFamily(invite.FamilyId);
            return IzumiReplyMessage.FamilyInviteListFamilyNullFieldDesc.Parse(
                _emotes.GetEmoteOrBlank("List"), invite.Id, family.Name);
        }

        /// <summary>
        /// Возвращает локализированную строку с информацией о отправленном приглашении в семью.
        /// </summary>
        /// <param name="invite">Приглашение.</param>
        /// <returns>Локализированная строка с информацией о отправленном приглашении в семью.</returns>
        private async Task<string> DisplayFamilyInvite(FamilyInviteModel invite)
        {
            // получаем пользователя, которому семья отправила приглашение
            var user = await _userService.GetUser(invite.UserId);
            return IzumiReplyMessage.FamilyInviteListFamilyNotNullFieldDesc.Parse(
                _emotes.GetEmoteOrBlank("List"), invite.Id,
                _emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name);
        }
    }
}
