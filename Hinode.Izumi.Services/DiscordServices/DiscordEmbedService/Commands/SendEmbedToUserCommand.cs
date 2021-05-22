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
}
