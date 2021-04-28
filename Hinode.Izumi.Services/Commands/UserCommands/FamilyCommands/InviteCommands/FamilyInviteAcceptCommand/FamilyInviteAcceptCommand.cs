using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.FamilyService;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteAcceptCommand
{
    [InjectableService]
    public class FamilyInviteAcceptCommand : IFamilyInviteAcceptCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IFamilyService _familyService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IUserService _userService;
        private readonly IEmoteService _emoteService;

        public FamilyInviteAcceptCommand(IDiscordEmbedService discordEmbedService, IFamilyService familyService,
            IDiscordGuildService discordGuildService, IUserService userService, IEmoteService emoteService)
        {
            _discordEmbedService = discordEmbedService;
            _familyService = familyService;
            _discordGuildService = discordGuildService;
            _userService = userService;
            _emoteService = emoteService;
        }

        public async Task Execute(SocketCommandContext context, long inviteId)
        {
            // получаем приглашение в семью
            var invite = await _familyService.GetFamilyInvite(inviteId);
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _familyService.CheckUserHasFamily((long) context.User.Id);

            if (hasFamily)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyAlready.Parse()));
            }
            else
            {
                // получаем иконки из базы
                var emotes = await _emoteService.GetEmotes();
                // получаем пользователя
                var user = await _userService.GetUser((long) context.User.Id);
                // получаем семью, в которую пользователя пригласили
                var family = await _familyService.GetFamily(invite.FamilyId);
                // получаем владельца этой семьи
                var familyOwner = await _familyService.GetFamilyOwner(family.Id);

                // удаляем приглашение
                await _familyService.RemoveFamilyInvite(inviteId);
                // добавляем пользователя в семью
                await _familyService.AddUserToFamily((long) context.User.Id, invite.FamilyId);

                var embed = new EmbedBuilder()
                    // подтверждаем что пользователь успешно принял приглашение
                    .WithDescription(IzumiReplyMessage.FamilyInviteAcceptSuccess.Parse(family.Name));

                await _discordEmbedService.SendEmbed(context.User, embed);

                var notify = new EmbedBuilder()
                    // оповещаем владельца семьи что пользователь принял приглашение
                    .WithDescription(IzumiReplyMessage.FamilyInviteAcceptSuccessNotify.Parse(
                        emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name));

                await _discordEmbedService.SendEmbed(
                    await _discordGuildService.GetSocketUser(familyOwner.Id), notify);
                await Task.CompletedTask;
            }
        }
    }
}
