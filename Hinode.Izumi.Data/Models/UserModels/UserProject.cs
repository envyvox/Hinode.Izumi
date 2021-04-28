namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Чертеж у пользователя.
    /// </summary>
    public class UserProject : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id чертежа.
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Чертеж.
        /// </summary>
        public virtual Project Project { get; set; }
    }
}
