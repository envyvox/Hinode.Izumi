using System;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Records
{
    public record UserAchievementRecord(long UserId, long AchievementId, DateTimeOffset CreatedAt)
    {
        public UserAchievementRecord() : this(default, default, default)
        {
        }
    }
}
