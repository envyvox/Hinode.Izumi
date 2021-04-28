namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Продукт у пользователя.
    /// </summary>
    public class UserProduct : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id продукта.
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// Количество продукта у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Продукт.
        /// </summary>
        public virtual Product Product { get; set; }
    }
}
