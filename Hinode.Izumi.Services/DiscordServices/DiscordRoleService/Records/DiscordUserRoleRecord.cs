using System;

namespace Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Records
{
    public record DiscordUserRoleRecord(long UserId, long RoleId, DateTimeOffset Expiration)
    {
        public DiscordUserRoleRecord() : this(default, default, default)
        {
        }
    }
}
