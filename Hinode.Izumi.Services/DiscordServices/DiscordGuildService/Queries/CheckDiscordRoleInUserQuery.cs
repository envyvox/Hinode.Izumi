using Hinode.Izumi.Data.Enums.DiscordEnums;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries
{
    public record CheckDiscordRoleInUserQuery(long UserId, DiscordRole Role) : IRequest<bool>;
}
