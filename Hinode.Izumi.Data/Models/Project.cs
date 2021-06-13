namespace Hinode.Izumi.Data.Models
{
    public class Project : EntityBase
    {
        public string Name { get; set; }
        public long Price { get; set; }
        public long Time { get; set; }
        public long? ReqBuildingId { get; set; }
        public virtual Building Building { get; set; }
    }
}
