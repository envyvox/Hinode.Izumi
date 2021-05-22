using System;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Commands
{
    public record AddDiscordRoleToUserToDbCommand(long UserId, long RoleId, DateTimeOffset Expiration) : IRequest;
}
