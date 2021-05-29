using System.Threading;
using System.Threading.Tasks;
using Discord;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries
{
    public record GetDiscordUserMessageQuery(long ChannelId, long MessageId) : IRequest<IUserMessage>;

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
