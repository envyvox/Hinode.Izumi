using System;
using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Commands
{
    public record CreateUserMovementCommand(
            long UserId,
            Location Departure,
            Location Destination,
            DateTimeOffset Arrival)
        : IRequest;
}
