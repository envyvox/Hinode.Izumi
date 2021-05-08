using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Models
{
    /// <summary>
    /// Сообщение в доске сообщества.
    /// </summary>
    public class ContentMessageModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id канала.
        /// </summary>
        public long ChannelId { get; set; }

        /// <summary>
        /// Id сообщения.
        /// </summary>
        public long MessageId { get; set; }
    }
}
