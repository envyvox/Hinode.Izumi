using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.DiscordModels
{
    /// <summary>
    /// Голос к сообщению в доске сообщества.
    /// </summary>
    public class ContentVote : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id (из базы) сообщения в доске сообщества.
        /// </summary>
        public long MessageId { get; set; }

        /// <summary>
        /// Тип голоса.
        /// </summary>
        public Vote Vote { get; set; }

        /// <summary>
        /// Активный голос?
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Сообщение в доске сообщества.
        /// </summary>
        public virtual ContentMessage Message { get; set; }
    }
}
