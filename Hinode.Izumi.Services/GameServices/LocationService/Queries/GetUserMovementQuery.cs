using Hinode.Izumi.Services.GameServices.LocationService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Queries
{
    public record GetUserMovementQuery(long UserId) : IRequest<MovementRecord>;
}
