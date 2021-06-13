using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserField : EntityBase
    {
        public long UserId { get; set; }
        public long FieldId { get; set; }
        public FieldState State { get; set; }
        public long? SeedId { get; set; }
        public long Progress { get; set; }
        public bool ReGrowth { get; set; }
        public virtual User User { get; set; }
        public virtual Seed Seed { get; set; }
    }
}
