using System;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Models
{
    /// <summary>
    /// Роль пользователя в дискорде.
    /// </summary>
    public class UserRoleModel : EntityBaseModel
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
