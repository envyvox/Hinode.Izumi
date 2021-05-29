using System;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.LocationService.Records
{
    public record MovementRecord(Location Departure, Location Destination, DateTimeOffset Arrival)
    {
        public MovementRecord() : this(default, default, default)
        {
        }
    }
}
