namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Постройка у пользователя.
    /// </summary>
    public class UserBuilding : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id постройки.
        /// </summary>
        public long BuildingId { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Постройка.
        /// </summary>
        public virtual Building Building { get; set; }
    }
}
