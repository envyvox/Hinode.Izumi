namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Алкоголь у пользователя.
    /// </summary>
    public class UserAlcohol : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id алкоголя.
        /// </summary>
        public long AlcoholId { get; set; }

        /// <summary>
        /// Количество алкоголя у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Алкоголь.
        /// </summary>
        public virtual Alcohol Alcohol { get; set; }
    }
}
