using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.FieldService.Models
{
    /// <summary>
    /// Клетка участка земли пользователя.
    /// </summary>
    public class UserFieldModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Номер клетки.
        /// </summary>
        public long? FieldId { get; set; }

        /// <summary>
        /// Состояние клетки земли.
        /// </summary>
        public FieldState State { get; set; }

        /// <summary>
        /// Id семени.
        /// </summary>
        public long SeedId { get; set; }

        /// <summary>
        /// Количество дней роста.
        /// </summary>
        public long Progress { get; set; }

        /// <summary>
        /// Отображение повторного роста.
        /// </summary>
        public bool ReGrowth { get; set; }
    }
}
