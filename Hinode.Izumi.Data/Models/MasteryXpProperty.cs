namespace Hinode.Izumi.Data.Models
{
    public class MasteryXpProperty : EntityBase
    {
        public Enums.PropertyEnums.MasteryXpProperty Property { get; set; }
        public double Mastery0 { get; set; }
        public double Mastery50 { get; set; }
        public double Mastery100 { get; set; }
        public double Mastery150 { get; set; }
        public double Mastery200 { get; set; }
        public double Mastery250 { get; set; }
    }
}
