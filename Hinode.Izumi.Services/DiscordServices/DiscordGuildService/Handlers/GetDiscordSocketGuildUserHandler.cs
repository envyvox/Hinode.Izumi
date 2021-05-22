using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Handlers
{
    public class GetDiscordSocketGuildUserHandler
        : IRequestHandler<GetDiscordSocketGuildUserQuery, SocketGuildUser>
    {
        private readonly IMediator _mediator;

        public GetDiscordSocketGuildUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<SocketGuildUser> Handle(GetDiscordSocketGuildUserQuery request,
            CancellationToken cancellationToken)
        {
            var socketGuild = await _mediator.Send(new GetDiscordSocketGuildQuery(), cancellationToken);
            return socketGuild.GetUser((ulong) request.Id);
        }
    }
}
