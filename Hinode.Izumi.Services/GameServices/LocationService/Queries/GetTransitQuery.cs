using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.LocationService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Queries
{
    public record GetTransitQuery(Location Departure, Location Destination) : IRequest<TransitRecord>;
}
