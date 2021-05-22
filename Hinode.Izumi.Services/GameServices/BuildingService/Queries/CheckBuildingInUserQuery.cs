using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Queries
{
    public record CheckBuildingInUserQuery(long UserId, Building Type) : IRequest<bool>;
}
