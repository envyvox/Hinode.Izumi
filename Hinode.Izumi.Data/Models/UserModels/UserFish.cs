namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserFish : EntityBase
    {
        public long UserId { get; set; }
        public long FishId { get; set; }
        public long Amount { get; set; }
        public virtual User User { get; set; }
        public virtual Fish Fish { get; set; }
    }
}
