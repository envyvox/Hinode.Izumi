using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Queries
{
    public record GetLocationRoleQuery(Location Location) : IRequest<DiscordRole>;
}
