using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Handlers
{
    public class SendEmbedToChannelHandler : IRequestHandler<SendEmbedToChannelCommand, IUserMessage>
    {
        private readonly IMediator _mediator;

        public SendEmbedToChannelHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IUserMessage> Handle(SendEmbedToChannelCommand request, CancellationToken cancellationToken)
        {
            var (channel, embedBuilder, message) = request;
            var channels = await _mediator.Send(new GetDiscordChannelsQuery(), cancellationToken);
            var textChannel = await _mediator.Send(
                new GetDiscordSocketTextChannelQuery(channels[channel].Id), cancellationToken);
            var embed = await _mediator.Send(new BuildEmbedCommand(embedBuilder), cancellationToken);

            return await textChannel.SendMessageAsync(message, false, embed);
        }
    }
}
