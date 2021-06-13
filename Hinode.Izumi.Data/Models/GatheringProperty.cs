namespace Hinode.Izumi.Data.Models
{
    public class GatheringProperty : EntityBase
    {
        public long GatheringId { get; set; }
        public Enums.PropertyEnums.GatheringProperty Property { get; set; }
        public long Mastery0 { get; set; }
        public long Mastery50 { get; set; }
        public long Mastery100 { get; set; }
        public long Mastery150 { get; set; }
        public long Mastery200 { get; set; }
        public long Mastery250 { get; set; }
        public virtual Gathering Gathering { get; set; }
    }
}
