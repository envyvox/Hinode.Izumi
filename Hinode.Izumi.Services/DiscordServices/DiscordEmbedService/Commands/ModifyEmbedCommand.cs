using Discord;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands
{
    public record ModifyEmbedCommand(IUserMessage Message, EmbedBuilder EmbedBuilder) : IRequest;
}
