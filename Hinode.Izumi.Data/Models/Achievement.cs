using Hinode.Izumi.Data.Enums.AchievementEnums;

namespace Hinode.Izumi.Data.Models
{
    public class Achievement : EntityBase
    {
        public AchievementCategory Category { get; set; }
        public Enums.AchievementEnums.Achievement Type { get; set; }
        public AchievementReward Reward { get; set; }
        public long Number { get; set; }
    }
}
