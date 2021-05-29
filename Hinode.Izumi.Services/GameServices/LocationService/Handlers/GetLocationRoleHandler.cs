using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Handlers
{
    public class GetLocationRoleHandler : IRequestHandler<GetLocationRoleQuery, DiscordRole>
    {
        public async Task<DiscordRole> Handle(GetLocationRoleQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(request.Location switch
            {
                Location.InTransit => DiscordRole.LocationInTransit,
                Location.Capital => DiscordRole.LocationCapital,
                Location.Garden => DiscordRole.LocationGarden,
                Location.Seaport => DiscordRole.LocationSeaport,
                Location.Castle => DiscordRole.LocationCastle,
                Location.Village => DiscordRole.LocationVillage,
                Location.CapitalCasino => DiscordRole.LocationCapital,
                Location.CapitalMarket => DiscordRole.LocationCapital,
                Location.CapitalShop => DiscordRole.LocationCapital,
                _ => throw new ArgumentOutOfRangeException()
            });
        }
    }
}
