namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Напиток у пользователя.
    /// </summary>
    public class UserDrink : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id напитка.
        /// </summary>
        public long DrinkId { get; set; }

        /// <summary>
        /// Количество напитка у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Напиток.
        /// </summary>
        public virtual Drink Drink { get; set; }
    }
}
