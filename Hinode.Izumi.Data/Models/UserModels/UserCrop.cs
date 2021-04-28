namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Урожай у пользователя.
    /// </summary>
    public class UserCrop : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id урожая.
        /// </summary>
        public long CropId { get; set; }

        /// <summary>
        /// Количество урожая у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Урожай.
        /// </summary>
        public virtual Crop Crop { get; set; }
    }
}
