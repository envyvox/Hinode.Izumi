using System;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Models.UserModels;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Перемещение пользователя.
    /// </summary>
    public class Movement : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Локация из которой отправляется пользователь.
        /// </summary>
        public Location Departure { get; set; }

        /// <summary>
        /// Локация куда прибывает пользователь.
        /// </summary>
        public Location Destination { get; set; }

        /// <summary>
        /// Дата прибытия пользователя.
        /// </summary>
        public DateTimeOffset Arrival { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }
    }
}
