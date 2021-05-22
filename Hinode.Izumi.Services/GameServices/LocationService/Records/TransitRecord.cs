using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.LocationService.Records
{
    public record TransitRecord(Location Departure, Location Destination, long Time, long Price)
    {
        public TransitRecord() : this(default, default, default, default)
        {
        }
    }
}
