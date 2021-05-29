using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Commands
{
    public record UpdateUserLocationCommand(long UserId, Location Location) : IRequest;
}
