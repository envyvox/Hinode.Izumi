namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserSeed : EntityBase
    {
        public long UserId { get; set; }
        public long SeedId { get; set; }
        public long Amount { get; set; }
        public virtual User User { get; set; }
        public virtual Seed Seed { get; set; }
    }
}
