using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.DiscordModels
{
    public class ContentVote : EntityBase
    {
        public long UserId { get; set; }
        public long MessageId { get; set; }
        public Vote Vote { get; set; }
        public bool Active { get; set; }
        public virtual ContentMessage Message { get; set; }
    }
}
