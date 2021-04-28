using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Коробка у пользователя.
    /// </summary>
    public class UserBox : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Коробка.
        /// </summary>
        public Box Box { get; set; }

        /// <summary>
        /// Количество коробок у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }
    }
}
