namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserBanner : EntityBase
    {
        public long UserId { get; set; }
        public long BannerId { get; set; }
        public bool Active { get; set; }
        public virtual User User { get; set; }
        public virtual Banner Banner { get; set; }
    }
}
