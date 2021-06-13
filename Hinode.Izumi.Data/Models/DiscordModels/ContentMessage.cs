namespace Hinode.Izumi.Data.Models.DiscordModels
{
    public class ContentMessage : EntityBase
    {
        public long UserId { get; set; }
        public long ChannelId { get; set; }
        public long MessageId { get; set; }
    }
}
