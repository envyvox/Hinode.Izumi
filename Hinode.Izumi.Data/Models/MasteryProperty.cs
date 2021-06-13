using Hinode.Izumi.Data.Enums.PropertyEnums;

namespace Hinode.Izumi.Data.Models
{
    public class MasteryProperty : EntityBase
    {
        public MasteryPropertyCategory PropertyCategory { get; set; }
        public Enums.PropertyEnums.MasteryProperty Property { get; set; }
        public long Mastery0 { get; set; }
        public long Mastery50 { get; set; }
        public long Mastery100 { get; set; }
        public long Mastery150 { get; set; }
        public long Mastery200 { get; set; }
        public long Mastery250 { get; set; }
    }
}
