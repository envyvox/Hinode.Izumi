namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Достижение у пользователя.
    /// </summary>
    public class UserAchievement : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id достижения.
        /// </summary>
        public long AchievementId { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Достижение.
        /// </summary>
        public virtual Achievement Achievement { get; set; }
    }
}
