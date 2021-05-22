using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands
{
    public record AddDiscordRoleToUserByRoleIdCommand(long UserId, long RoleId) : IRequest;
}
