namespace Hinode.Izumi.Data.Models.DiscordModels
{
    /// <summary>
    /// Сообщение в доске сообщества.
    /// </summary>
    public class ContentMessage : EntityBase
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
