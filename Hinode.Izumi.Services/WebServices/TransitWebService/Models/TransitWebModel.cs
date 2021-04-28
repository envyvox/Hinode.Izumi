using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.TransitWebService.Models
{
    /// <summary>
    /// Отправление.
    /// </summary>
    public class TransitWebModel : EntityBaseModel
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
