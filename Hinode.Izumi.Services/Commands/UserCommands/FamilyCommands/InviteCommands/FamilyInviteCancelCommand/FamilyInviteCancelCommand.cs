using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.RpgServices.FamilyService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteCancelCommand
{
    [InjectableService]
    public class FamilyInviteCancelCommand : IFamilyInviteCancelCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IFamilyService _familyService;
        private readonly IDiscordGuildService _discordGuildService;

        public FamilyInviteCancelCommand(IDiscordEmbedService discordEmbedService, IFamilyService familyService,
            IDiscordGuildService discordGuildService)
        {
            _discordEmbedService = discordEmbedService;
            _familyService = familyService;
            _discordGuildService = discordGuildService;
        }

        public async Task Execute(SocketCommandContext context, long inviteId)
        {
            // получаем пользователя в семье
            var userFamily = await _familyService.GetUserFamily((long) context.User.Id);

            // проверяем что пользователь является главой семьи
            if (userFamily.Status != UserInFamilyStatus.Head)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyStatusRequireHead.Parse()));
            }
            else
            {
                // получаем приглашение
                var invite = await _familyService.GetFamilyInvite(inviteId);
                // получаем семью
                var family = await _familyService.GetFamily(invite.FamilyId);

                // удаляем приглашение в семью
                await _familyService.RemoveFamilyInvite(inviteId);

                var embed = new EmbedBuilder()
                    // подтверждаем что приглашение успешно отменено
                    .WithDescription(IzumiReplyMessage.FamilyInviteCancelSuccess.Parse(invite.Id));

                await _discordEmbedService.SendEmbed(context.User, embed);

                var notify = new EmbedBuilder()
                    // оповещаем пользователя что его приглашение в семью отменено
                    .WithDescription(IzumiReplyMessage.FamilyInviteCancelSuccessNotify.Parse(family.Name));

                await _discordEmbedService.SendEmbed(
                    await _discordGuildService.GetSocketUser(invite.UserId), notify);
                await Task.CompletedTask;
            }
        }
    }
}
