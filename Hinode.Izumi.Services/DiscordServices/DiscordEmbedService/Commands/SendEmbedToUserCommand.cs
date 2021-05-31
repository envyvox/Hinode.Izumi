using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands
{
    public record SendEmbedToUserCommand(
            SocketUser SocketUser,
            EmbedBuilder EmbedBuilder,
            string Message = "")
        : IRequest<IUserMessage>;

    public class SendEmbedToUserHandler : IRequestHandler<SendEmbedToUserCommand, IUserMessage>
    {
        private readonly IMediator _mediator;

        public SendEmbedToUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IUserMessage> Handle(SendEmbedToUserCommand request, CancellationToken ct)
        {
            var (socketUser, embedBuilder, message) = request;
            var embed = await _mediator.Send(new BuildEmbedCommand(embedBuilder, (long) socketUser.Id), ct);

            return await socketUser.SendMessageAsync(message, false, embed);
        }
    }
}
