namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Records
{
    public record ContentMessageRecord(
        long Id,
        long UserId,
        long ChannelId,
        long MessageId)
    {
        public ContentMessageRecord() : this(default, default, default, default)
        {
        }
    }
}
