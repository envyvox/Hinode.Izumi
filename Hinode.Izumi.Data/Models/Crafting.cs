using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class Crafting : EntityBase
    {
        public string Name { get; set; }
        public long Time { get; set; }
        public Location Location { get; set; }
    }
}
