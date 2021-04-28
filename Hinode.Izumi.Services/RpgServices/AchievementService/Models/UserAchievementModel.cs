using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.AchievementService.Models
{
    /// <summary>
    /// Достижение у пользователя.
    /// </summary>
    public class UserAchievementModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id достижения.
        /// </summary>
        public long AchievementId { get; set; }
    }
}
