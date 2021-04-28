using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.FamilyService;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteSendCommand
{
    [InjectableService]
    public class FamilyInviteSendCommand : IFamilyInviteSendCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IUserService _userService;
        private readonly IFamilyService _familyService;

        public FamilyInviteSendCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IDiscordGuildService discordGuildService, IUserService userService, IFamilyService familyService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _discordGuildService = discordGuildService;
            _userService = userService;
            _familyService = familyService;
        }

        public async Task Execute(SocketCommandContext context, string username)
        {
            // получаем пользователя в семье
            var userFamily = await _familyService.GetUserFamily((long) context.User.Id);

            // проверяем что пользователь является владельцем семьи
            if (userFamily.Status != UserInFamilyStatus.Head)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyStatusRequireHead.Parse()));
            }
            else
            {
                // получаем пользователя цель
                var tUser = await _userService.GetUser(username);
                // получаем информацию о том, в какой семье состоит цель
                var tUserFamily = await _familyService.GetUserFamily(tUser.Id);

                // проверяем что цель не состоит в семье
                if (tUserFamily != null)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyNotNull.Parse()));
                }
                else
                {
                    // получаем приглашение в семью для этой цели
                    var invite = await _familyService.GetFamilyInvite(userFamily.FamilyId, tUser.Id);

                    // проверяем не отправляла ли уже семья приглашение этому пользователю
                    if (invite != null)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.FamilyInviteSendAlready.Parse()));
                    }
                    else
                    {
                        // получаем иконки из базы
                        var emotes = await _emoteService.GetEmotes();
                        // получаем семью
                        var family = await _familyService.GetFamily(userFamily.FamilyId);

                        // создаем приглашение
                        await _familyService.AddFamilyInvite(userFamily.FamilyId, tUser.Id);

                        var embed = new EmbedBuilder()
                            // подтверждаем что приглашение успешно создано
                            .WithDescription(IzumiReplyMessage.FamilyInviteSendSuccess.Parse(
                                emotes.GetEmoteOrBlank(tUser.Title.Emote()), tUser.Title.Localize(),
                                tUser.Name));

                        await _discordEmbedService.SendEmbed(context.User, embed);

                        var notify = new EmbedBuilder()
                            // оповещаем цель о том, что ее пригласили в семью
                            .WithDescription(IzumiReplyMessage.FamilyInviteSendSuccessNotify.Parse(family.Name));

                        await _discordEmbedService.SendEmbed(
                            await _discordGuildService.GetSocketUser(tUser.Id), notify);
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
