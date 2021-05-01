using System;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.CooldownService.Models
{
    /// <summary>
    /// Кулдаун у пользователя.
    /// </summary>
    public class UserCooldownModel : EntityBaseModel
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
        public DateTimeOffset Expiration { get; set; }
    }
}
