namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Баннер у пользователя.
    /// </summary>
    public class UserBanner : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id баннера.
        /// </summary>
        public long BannerId { get; set; }

        /// <summary>
        /// Текущий статус баннера.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Баннер.
        /// </summary>
        public virtual Banner Banner { get; set; }
    }
}
