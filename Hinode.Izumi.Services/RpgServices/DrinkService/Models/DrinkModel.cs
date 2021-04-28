using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.DrinkService.Models
{
    /// <summary>
    /// Напиток.
    /// </summary>
    public class DrinkModel : EntityBaseModel
    {
        /// <summary>
        /// Название напитка.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Длительность изготовления.
        /// </summary>
        public long Time { get; set; }
    }
}
