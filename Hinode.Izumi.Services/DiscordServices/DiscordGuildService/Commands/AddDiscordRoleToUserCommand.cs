using Hinode.Izumi.Data.Enums.DiscordEnums;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands
{
    public record AddDiscordRoleToUserCommand(long UserId, DiscordRole Role) : IRequest;
}
