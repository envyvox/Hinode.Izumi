using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries
{
    public record GetDiscordSocketTextChannelQuery(long Id) : IRequest<SocketTextChannel>;

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

            try
            {
                return socketGuild.GetTextChannel((ulong) request.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}
