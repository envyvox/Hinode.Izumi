using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class Seed : EntityBase
    {
        public string Name { get; set; }
        public Season Season { get; set; }
        public long Growth { get; set; }
        public long ReGrowth { get; set; }
        public long Price { get; set; }
        public bool Multiply { get; set; }
    }
}
