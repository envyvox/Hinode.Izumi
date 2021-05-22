using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Records;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Queries
{
    public record GetExpiredDiscordUserRolesQuery : IRequest<DiscordUserRoleRecord[]>;
}
