using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Клетка участка земли пользователя.
    /// </summary>
    public class UserField : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Номер клетки.
        /// </summary>
        public long FieldId { get; set; }

        /// <summary>
        /// Состояние клетки земли.
        /// </summary>
        public FieldState State { get; set; }

        /// <summary>
        /// Id семени.
        /// </summary>
        public long? SeedId { get; set; }

        /// <summary>
        /// Количество дней роста.
        /// </summary>
        public long Progress { get; set; }

        /// <summary>
        /// Отображение повторного роста.
        /// </summary>
        public bool ReGrowth { get; set; }

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
