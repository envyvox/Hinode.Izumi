using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class Transit : EntityBase
    {
        public Location Departure { get; set; }
        public Location Destination { get; set; }
        public long Time { get; set; }
        public long Price { get; set; }
    }
}
