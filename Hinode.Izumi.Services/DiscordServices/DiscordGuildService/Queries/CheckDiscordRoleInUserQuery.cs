using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries
{
    public record CheckDiscordRoleInUserQuery(long UserId, DiscordRole Role) : IRequest<bool>;

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
