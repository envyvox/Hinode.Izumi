namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Семя у пользователя.
    /// </summary>
    public class UserSeed : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id семян.
        /// </summary>
        public long SeedId { get; set; }

        /// <summary>
        /// Количество семян у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Семя.
        /// </summary>
        public virtual Seed Seed { get; set; }
    }
}
