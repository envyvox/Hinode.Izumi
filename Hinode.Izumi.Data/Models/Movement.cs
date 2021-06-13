using System;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Models.UserModels;

namespace Hinode.Izumi.Data.Models
{
    public class Movement : EntityBase
    {
        public long UserId { get; set; }
        public Location Departure { get; set; }
        public Location Destination { get; set; }
        public DateTimeOffset Arrival { get; set; }
        public virtual User User { get; set; }
    }
}
