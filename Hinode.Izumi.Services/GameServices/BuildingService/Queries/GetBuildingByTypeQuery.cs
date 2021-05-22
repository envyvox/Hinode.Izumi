using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.BuildingService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Queries
{
    public record GetBuildingByTypeQuery(Building Type) : IRequest<BuildingRecord>;
}
