using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class CurrentWeeklyQuest : EntityBase
    {
        public Location Location { get; set; }
        public long QuestId { get; set; }
        public virtual WeeklyQuest Quest { get; set; }
    }
}
