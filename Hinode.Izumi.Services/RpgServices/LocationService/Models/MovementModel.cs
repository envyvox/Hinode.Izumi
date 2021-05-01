using System;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.LocationService.Models
{
    /// <summary>
    /// Перемещение пользователя.
    /// </summary>
    public class MovementModel : EntityBaseModel
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
    }
}
