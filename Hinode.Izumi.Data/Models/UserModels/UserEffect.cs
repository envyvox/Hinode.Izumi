using System;
using Hinode.Izumi.Data.Enums.EffectEnums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Эффект у пользователя.
    /// </summary>
    public class UserEffect : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Категория эффекта.
        /// </summary>
        public EffectCategory Category { get; set; }

        /// <summary>
        /// Эффект.
        /// </summary>
        public Effect Effect { get; set; }

        /// <summary>
        /// Окончание эффекта.
        /// </summary>
        public DateTimeOffset? Expiration { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }
    }
}
