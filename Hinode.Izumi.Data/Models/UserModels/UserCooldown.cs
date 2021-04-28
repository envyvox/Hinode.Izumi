using System;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Кулдаун у пользователя.
    /// </summary>
    public class UserCooldown : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Кулдаун.
        /// </summary>
        public Cooldown Cooldown { get; set; }

        /// <summary>
        /// Дата окончания кулдауна.
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }
    }
}
