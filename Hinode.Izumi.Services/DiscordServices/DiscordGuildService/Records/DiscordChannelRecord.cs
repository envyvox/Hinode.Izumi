using Hinode.Izumi.Data.Enums.DiscordEnums;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Records
{
    public record DiscordChannelRecord(long Id, DiscordChannel Channel)
    {
        public DiscordChannelRecord() : this(default, default)
        {
        }
    }
}
