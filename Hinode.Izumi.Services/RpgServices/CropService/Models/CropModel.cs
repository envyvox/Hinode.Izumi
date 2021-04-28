using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.CropService.Models
{
    /// <summary>
    /// Урожай, который вырастает из семян.
    /// </summary>
    public class CropModel : EntityBaseModel
    {
        /// <summary>
        /// Название урожая.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Цена урожая.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Id семени, из которого вырастает этот урожай.
        /// </summary>
        public long SeedId { get; set; }

        /// <summary>
        /// Сезон в котором растет урожай.
        /// </summary>
        public Season Season { get; set; }
    }
}
