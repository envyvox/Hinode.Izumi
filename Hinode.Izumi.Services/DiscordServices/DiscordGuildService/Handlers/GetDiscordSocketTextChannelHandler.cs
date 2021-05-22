using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Handlers
{
    public class GetDiscordSocketTextChannelHandler
        : IRequestHandler<GetDiscordSocketTextChannelQuery, SocketTextChannel>
    {
        private readonly IMediator _mediator;

        public GetDiscordSocketTextChannelHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<SocketTextChannel> Handle(GetDiscordSocketTextChannelQuery request,
            CancellationToken cancellationToken)
        {
            var socketGuild = await _mediator.Send(new GetDiscordSocketGuildQuery(), cancellationToken);
            return socketGuild.GetTextChannel((ulong) request.Id);
        }
    }
}
