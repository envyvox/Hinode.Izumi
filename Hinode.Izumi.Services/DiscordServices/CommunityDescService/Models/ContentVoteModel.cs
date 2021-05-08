using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Models
{
    /// <summary>
    /// Голос к сообщению в доске сообщества.
    /// </summary>
    public class ContentVoteModel : EntityBaseModel
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
    }
}
