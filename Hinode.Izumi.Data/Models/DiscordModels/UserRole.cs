using System;

namespace Hinode.Izumi.Data.Models.DiscordModels
{
    public class UserRole : EntityBase
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public DateTimeOffset Expiration { get; set; }
    }
}
