using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Handlers
{
    public class CheckDiscordRoleInUserHandler : IRequestHandler<CheckDiscordRoleInUserQuery, bool>
    {
        private readonly IMediator _mediator;

        public CheckDiscordRoleInUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(CheckDiscordRoleInUserQuery request, CancellationToken cancellationToken)
        {
            var (userId, role) = request;
            var user = await _mediator.Send(new GetDiscordSocketGuildUserQuery(userId), cancellationToken);
            var roles = await _mediator.Send(new GetDiscordRolesQuery(), cancellationToken);

            return user.Roles.Any(x => x.Id == (ulong) roles[role].Id);
        }
    }
}
