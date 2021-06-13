namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserAchievement : EntityBase
    {
        public long UserId { get; set; }
        public long AchievementId { get; set; }
        public virtual User User { get; set; }
        public virtual Achievement Achievement { get; set; }
    }
}
