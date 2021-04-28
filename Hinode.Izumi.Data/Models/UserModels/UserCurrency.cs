using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Валюта пользователя.
    /// </summary>
    public class UserCurrency : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Количество валюты у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }
    }
}
