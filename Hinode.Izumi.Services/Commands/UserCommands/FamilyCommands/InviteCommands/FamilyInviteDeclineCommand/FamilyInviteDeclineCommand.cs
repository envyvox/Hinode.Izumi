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

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteDeclineCommand
{
    [InjectableService]
    public class FamilyInviteDeclineCommand : IFamilyInviteDeclineCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IFamilyService _familyService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IUserService _userService;
        private readonly IEmoteService _emoteService;

        public FamilyInviteDeclineCommand(IDiscordEmbedService discordEmbedService, IFamilyService familyService,
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
            // получаем приглашение
            var invite = await _familyService.GetFamilyInvite(inviteId);
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем пользователя
            var user = await _userService.GetUser((long) context.User.Id);
            // получаем семью
            var family = await _familyService.GetFamily(invite.FamilyId);

            // удаляем приглашение
            await _familyService.RemoveFamilyInvite(inviteId);

            var embed = new EmbedBuilder()
                // подтверждаем что приглашение успешно отклонено
                .WithDescription(IzumiReplyMessage.FamilyInviteDeclineSuccess.Parse(family.Name));

            await _discordEmbedService.SendEmbed(context.User, embed);

            // получаем владельца семьи
            var familyOwner = await _familyService.GetFamilyOwner(invite.FamilyId);
            var notify = new EmbedBuilder()
                // оповещаем владельца семьи о том, что приглашение было отклонено
                .WithDescription(IzumiReplyMessage.FamilyInviteDeclineSuccessNotify.Parse(
                    emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name));

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(familyOwner.Id), notify);
            await Task.CompletedTask;
        }
    }
}
