using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Queries
{
    public record CheckBuildingInFamilyQuery(long FamilyId, Building Type) : IRequest<bool>;
}
