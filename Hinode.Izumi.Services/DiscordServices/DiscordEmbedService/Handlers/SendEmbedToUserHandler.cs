using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Handlers
{
    public class SendEmbedToUserHandler : IRequestHandler<SendEmbedToUserCommand, IUserMessage>
    {
        private readonly IMediator _mediator;

        public SendEmbedToUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IUserMessage> Handle(SendEmbedToUserCommand request, CancellationToken cancellationToken)
        {
            var (socketUser, embedBuilder, message) = request;
            var embed = await _mediator.Send(new BuildEmbedCommand(embedBuilder), cancellationToken);

            return await socketUser.SendMessageAsync(message, false, embed);
        }
    }
}
