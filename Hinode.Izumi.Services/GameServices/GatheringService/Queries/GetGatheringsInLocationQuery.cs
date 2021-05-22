using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.GatheringService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.GatheringService.Queries
{
    public record GetGatheringsInLocationQuery(Location Location) : IRequest<GatheringRecord[]>;
}
