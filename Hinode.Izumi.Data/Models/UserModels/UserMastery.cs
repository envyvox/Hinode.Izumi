using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Мастерство пользователя.
    /// </summary>
    public class UserMastery : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Мастерство.
        /// </summary>
        public Mastery Mastery { get; set; }

        /// <summary>
        /// Количество мастерства у пользователя.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }
    }
}
