namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Рыба у пользователя.
    /// </summary>
    public class UserFish : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id рыбы.
        /// </summary>
        public long FishId { get; set; }

        /// <summary>
        /// Количество рыбы у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Рыба.
        /// </summary>
        public virtual Fish Fish { get; set; }
    }
}
