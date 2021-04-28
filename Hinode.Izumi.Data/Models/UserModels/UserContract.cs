namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Контракт которым занимается пользователь.
    /// </summary>
    public class UserContract : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id контракта.
        /// </summary>
        public long ContractId { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Контракт.
        /// </summary>
        public virtual Contract Contract { get; set; }
    }
}
