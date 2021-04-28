using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Шаг обучения у пользователя.
    /// </summary>
    public class UserTraining : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Шаг обучения.
        /// </summary>
        public TrainingStep Step { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }
    }
}
