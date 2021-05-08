using System;

namespace Hinode.Izumi.Data.Models.DiscordModels
{
    /// <summary>
    /// Роль пользователя в дискорде.
    /// </summary>
    public class UserRole : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id роли.
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// Срок действия.
        /// </summary>
        public DateTimeOffset Expiration { get; set; }
    }
}
