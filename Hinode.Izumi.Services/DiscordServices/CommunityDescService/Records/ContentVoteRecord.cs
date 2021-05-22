using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Records
{
    public record ContentVoteRecord(
        long Id,
        long UserId,
        long MessageId,
        Vote Vote,
        bool Active)
    {
        public ContentVoteRecord() : this(default, default, default, default, default)
        {
        }
    }
}
