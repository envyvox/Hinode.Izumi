using System.Threading.Tasks;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Queries;
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
            // получаем сообщение
            var message = await _mediator.Send(new GetDiscordUserMessageQuery(channelId, messageId));
            // удаляем его
            await message.DeleteAsync();
        }

        public async Task RemoveExpiredRoleFromUsers()
        {
            // получаем роли пользователей с истекшим сроком
            var expiredRoles = await _mediator.Send(new GetExpiredDiscordUserRolesQuery());

            // если такое роли есть
            if (expiredRoles.Length > 0)
            {
                // снимаем каждую из них
                foreach (var expiredRole in expiredRoles)
                    await _mediator.Send(new RemoveDiscordRoleFromUserByRoleIdCommand(
                        expiredRole.UserId, expiredRole.RoleId));
            }
        }
    }
}
