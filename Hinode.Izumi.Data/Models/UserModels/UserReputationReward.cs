using Hinode.Izumi.Data.Enums.ReputationEnums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Награда за достижение репутации.
    /// </summary>
    public class UserReputationReward : EntityBase
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
        /// Количество репутации (брекет) за который пользователь получил награду.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }
    }
}
