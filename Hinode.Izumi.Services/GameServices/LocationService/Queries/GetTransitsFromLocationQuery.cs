using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.LocationService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Queries
{
    public record GetTransitsFromLocationQuery(Location Departure) : IRequest<TransitRecord[]>;
}
