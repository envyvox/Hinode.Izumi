using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Queries
{
    public record GetUserLocationQuery(long UserId) : IRequest<Location>;
}
