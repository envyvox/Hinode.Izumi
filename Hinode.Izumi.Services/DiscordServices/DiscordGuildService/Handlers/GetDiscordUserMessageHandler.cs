using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Handlers
{
    public class GetDiscordUserMessageHandler : IRequestHandler<GetDiscordUserMessageQuery, IUserMessage>
    {
        private readonly IMediator _mediator;

        public GetDiscordUserMessageHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IUserMessage> Handle(GetDiscordUserMessageQuery request, CancellationToken cancellationToken)
        {
            var (channelId, messageId) = request;
            var textChannel = await _mediator.Send(new GetDiscordSocketTextChannelQuery(channelId),
                cancellationToken);

            return (IUserMessage) await textChannel.GetMessageAsync((ulong) messageId);
        }
    }
}
