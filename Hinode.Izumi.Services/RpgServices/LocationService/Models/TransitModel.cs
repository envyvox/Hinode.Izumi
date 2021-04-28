using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.LocationService.Models
{
    /// <summary>
    /// Отправление.
    /// </summary>
    public class TransitModel : EntityBaseModel
    {
        /// <summary>
        /// Локация из которой отправляется пользователь.
        /// </summary>
        public Location Departure { get; set; }

        /// <summary>
        /// Локация куда прибывает пользователь.
        /// </summary>
        public Location Destination { get; set; }

        /// <summary>
        /// Длительность поездки.
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Стоимость отправления.
        /// </summary>
        public long Price { get; set; }
    }
}
