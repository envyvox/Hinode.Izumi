using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands
{
    public record AddDiscordRoleToUserByRoleIdCommand(long UserId, long RoleId) : IRequest;

    public class AddDiscordRoleToUserByRoleIdHandler : IRequestHandler<AddDiscordRoleToUserByRoleIdCommand>
    {
        private readonly IMediator _mediator;

        public AddDiscordRoleToUserByRoleIdHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddDiscordRoleToUserByRoleIdCommand request, CancellationToken cancellationToken)
        {
            var (userId, roleId) = request;
            var socketGuild = await _mediator.Send(new GetDiscordSocketGuildQuery(), cancellationToken);
            var socketGuildUser = await _mediator.Send(new GetDiscordSocketGuildUserQuery(userId), cancellationToken);
            var socketRole = socketGuild.GetRole((ulong) roleId);

            await socketGuildUser.AddRoleAsync(socketRole);

            return new Unit();
        }
    }
}
