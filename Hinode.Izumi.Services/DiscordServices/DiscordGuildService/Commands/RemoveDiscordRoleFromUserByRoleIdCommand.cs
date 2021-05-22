using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands
{
    public record RemoveDiscordRoleFromUserByRoleIdCommand(long UserId, long RoleId) : IRequest;
}
