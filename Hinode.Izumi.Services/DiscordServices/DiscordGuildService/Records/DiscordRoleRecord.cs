using Hinode.Izumi.Data.Enums.DiscordEnums;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Records
{
    public record DiscordRoleRecord(long Id, DiscordRole Role)
    {
        public DiscordRoleRecord() : this(default, default)
        {
        }
    }
}
