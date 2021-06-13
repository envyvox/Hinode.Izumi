using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class Contract : EntityBase
    {
        public Location Location { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Time { get; set; }
        public long Currency { get; set; }
        public long Reputation { get; set; }
        public long Energy { get; set; }
    }
}
