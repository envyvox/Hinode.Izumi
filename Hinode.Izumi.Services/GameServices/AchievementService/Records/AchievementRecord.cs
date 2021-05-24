using Hinode.Izumi.Data.Enums.AchievementEnums;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Records
{
    public record AchievementRecord(
        long Id,
        AchievementCategory Category,
        Achievement Type,
        AchievementReward Reward,
        long Number)
    {
        public AchievementRecord() : this(default, default, default, default, default)
        {
        }
    }
}
