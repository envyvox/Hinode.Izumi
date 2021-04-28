namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Блюдо у пользователя.
    /// </summary>
    public class UserFood : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id блюда.
        /// </summary>
        public long FoodId { get; set; }

        /// <summary>
        /// Количество блюда у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Блюдо.
        /// </summary>
        public virtual Food Food { get; set; }
    }
}
