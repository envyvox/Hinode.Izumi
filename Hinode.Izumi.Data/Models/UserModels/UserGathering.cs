namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Собирательский ресурс у пользователя.
    /// </summary>
    public class UserGathering : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id собирательского ресурса.
        /// </summary>
        public long GatheringId { get; set; }

        /// <summary>
        /// Количество собирательского ресурса у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Собирательский ресурс.
        /// </summary>
        public virtual Gathering Gathering { get; set; }
    }
}
