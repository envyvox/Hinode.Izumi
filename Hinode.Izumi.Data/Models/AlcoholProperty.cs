namespace Hinode.Izumi.Data.Models
{
    public class AlcoholProperty : EntityBase
    {
        public long AlcoholId { get; set; }
        public Enums.PropertyEnums.AlcoholProperty Property { get; set; }
        public long Mastery0 { get; set; }
        public long Mastery50 { get; set; }
        public long Mastery100 { get; set; }
        public long Mastery150 { get; set; }
        public long Mastery200 { get; set; }
        public long Mastery250 { get; set; }
        public virtual Alcohol Alcohol { get; set; }
    }
}
