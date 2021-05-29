using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Commands
{
    public record BeginUserTransitCommand(long UserId, Location Departure, Location Destination, long Time) : IRequest;
}
