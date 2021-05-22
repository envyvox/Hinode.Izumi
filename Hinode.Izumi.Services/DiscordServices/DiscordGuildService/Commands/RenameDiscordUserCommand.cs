using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands
{
    public record RenameDiscordUserCommand(long Id, string NewName) : IRequest;
}
