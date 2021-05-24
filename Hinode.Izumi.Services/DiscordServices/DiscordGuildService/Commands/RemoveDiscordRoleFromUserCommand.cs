using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands
{
    public record RemoveDiscordRoleFromUserCommand(long UserId, DiscordRole Role) : IRequest;

    public class RemoveDiscordRoleFromUserHandler : IRequestHandler<RemoveDiscordRoleFromUserCommand>
    {
        private readonly IMediator _mediator;

        public RemoveDiscordRoleFromUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveDiscordRoleFromUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, role) = request;
            var socketGuild = await _mediator.Send(new GetDiscordSocketGuildQuery(), cancellationToken);
            var socketGuildUser = await _mediator.Send(new GetDiscordSocketGuildUserQuery(userId), cancellationToken);
            var discordRoles = await _mediator.Send(new GetDiscordRolesQuery(), cancellationToken);
            var socketRole = socketGuild.GetRole((ulong) discordRoles[role].Id);

            await socketGuildUser.RemoveRoleAsync(socketRole);

            return new Unit();
        }
    }
}
