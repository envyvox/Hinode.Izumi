using Hinode.Izumi.Services.GameServices.BuildingService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Queries
{
    public record GetBuildingByProjectIdQuery(long ProjectId) : IRequest<BuildingRecord>;
}
