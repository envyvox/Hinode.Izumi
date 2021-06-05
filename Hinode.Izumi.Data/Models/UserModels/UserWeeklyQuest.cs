namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserWeeklyQuest : EntityBase
    {
        public long UserId { get; set; }
        public long QuestId { get; set; }
        public virtual User User { get; set; }
        public virtual CurrentWeeklyQuest Quest { get; set; }
    }
}
