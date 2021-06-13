namespace Hinode.Izumi.Data.Models
{
    public class Crop : EntityBase
    {
        public string Name { get; set; }
        public long Price { get; set; }
        public long SeedId { get; set; }
        public virtual Seed Seed { get; set; }
    }
}
