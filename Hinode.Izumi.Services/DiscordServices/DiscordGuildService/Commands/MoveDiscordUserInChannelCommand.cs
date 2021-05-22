using Discord.Rest;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands
{
    public record MoveDiscordUserInChannelCommand(long UserId, RestVoiceChannel Channel) : IRequest;
}
