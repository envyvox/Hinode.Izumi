using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Queries;
using Hinode.Izumi.Services.GameServices.PremiumService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.BackgroundJobs.DiscordJob
{
    [InjectableService]
    public class DiscordJob : IDiscordJob
    {
        private readonly IMediator _mediator;

        public DiscordJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DeleteMessage(long channelId, long messageId)
        {
            var message = await _mediator.Send(new GetDiscordUserMessageQuery(channelId, messageId));
            await message.DeleteAsync();
        }

        public async Task RemoveExpiredRoleFromUsers()
        {
            var roles = await _mediator.Send(new GetDiscordRolesQuery());
            var expiredRoles = await _mediator.Send(new GetExpiredDiscordUserRolesQuery());

            if (expiredRoles.Length > 0)
            {
                foreach (var expiredRole in expiredRoles)
                {
                    await _mediator.Send(new RemoveDiscordRoleFromUserByRoleIdCommand(
                        expiredRole.UserId, expiredRole.RoleId));
                    await _mediator.Send(new DeleteDiscordUserRoleFromDbCommand(
                        expiredRole.UserId, expiredRole.RoleId));

                    if (expiredRole.RoleId == roles[DiscordRole.Premium].Id)
                        await _mediator.Send(new DeactivateUserPremiumCommand(expiredRole.UserId));
                }
            }
        }
    }
}
