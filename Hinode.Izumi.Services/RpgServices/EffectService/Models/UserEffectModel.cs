using System;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.EffectService.Models
{
    /// <summary>
    /// Эффект у пользователя.
    /// </summary>
    public class UserEffectModel : EntityBaseModel
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
        public DateTime? Expiration { get; set; }
    }
}
