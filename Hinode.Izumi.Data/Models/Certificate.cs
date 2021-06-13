namespace Hinode.Izumi.Data.Models
{
    public class Certificate : EntityBase
    {
        public Enums.Certificate Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
    }
}
