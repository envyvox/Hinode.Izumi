using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Models
{
    public class DiscordRoleModel : EntityBaseModel
    {
        /// <summary>
        /// Роль дискорда.
        /// </summary>
        public DiscordRole Role { get; set; }
    }
}
