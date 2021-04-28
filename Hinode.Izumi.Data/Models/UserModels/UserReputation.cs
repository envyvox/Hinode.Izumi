using Hinode.Izumi.Data.Enums.ReputationEnums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Репутация у пользователя.
    /// </summary>
    public class UserReputation : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Репутация.
        /// </summary>
        public Reputation Reputation { get; set; }

        /// <summary>
        /// Количество репутации у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }
    }
}
