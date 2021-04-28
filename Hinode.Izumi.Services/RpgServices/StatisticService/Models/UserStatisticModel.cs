using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.StatisticService.Models
{
    /// <summary>
    /// Статистика пользователя.
    /// </summary>
    public class UserStatisticModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Статистика.
        /// </summary>
        public Statistic Statistic { get; set; }

        /// <summary>
        /// Количество действий.
        /// </summary>
        public long Amount { get; set; }
    }
}
