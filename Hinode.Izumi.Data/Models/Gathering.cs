using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class Gathering : EntityBase
    {
        public string Name { get; set; }
        public long Price { get; set; }
        public Location Location { get; set; }
        public Event Event { get; set; }
    }
}
