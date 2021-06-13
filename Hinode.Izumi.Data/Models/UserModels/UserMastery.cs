using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserMastery : EntityBase
    {
        public long UserId { get; set; }
        public Mastery Mastery { get; set; }
        public double Amount { get; set; }
        public virtual User User { get; set; }
    }
}
