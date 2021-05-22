using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Handlers
{
    public class RemoveDiscordRoleFromUserByRoleIdHandler : IRequestHandler<RemoveDiscordRoleFromUserByRoleIdCommand>
    {
        private readonly IMediator _mediator;

        public RemoveDiscordRoleFromUserByRoleIdHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveDiscordRoleFromUserByRoleIdCommand request,
            CancellationToken cancellationToken)
        {
            var (userId, roleId) = request;
            var socketGuild = await _mediator.Send(new GetDiscordSocketGuildQuery(), cancellationToken);
            var socketGuildUser = await _mediator.Send(new GetDiscordSocketGuildUserQuery(userId), cancellationToken);
            var socketRole = socketGuild.GetRole((ulong) roleId);

            await socketGuildUser.RemoveRoleAsync(socketRole);

            return new Unit();
        }
    }
}
