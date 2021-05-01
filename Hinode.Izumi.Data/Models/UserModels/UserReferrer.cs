namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Реферрер пользователя.
    /// </summary>
    public class UserReferrer : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id реферрера.
        /// </summary>
        public long ReferrerId { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Реферрер.
        /// </summary>
        public virtual User Referrer { get; set; }
    }
}
