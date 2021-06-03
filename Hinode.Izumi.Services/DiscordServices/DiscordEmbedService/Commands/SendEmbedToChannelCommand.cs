using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands
{
    public record SendEmbedToChannelCommand(
            DiscordChannel Channel,
            EmbedBuilder EmbedBuilder,
            string Message = "")
        : IRequest<IUserMessage>;

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

            try
            {
                return await textChannel.SendMessageAsync(message, false, embed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}
