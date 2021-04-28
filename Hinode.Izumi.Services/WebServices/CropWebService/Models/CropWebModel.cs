using Hinode.Izumi.Services.Database;
using Hinode.Izumi.Services.WebServices.SeedWebService.Models;

namespace Hinode.Izumi.Services.WebServices.CropWebService.Models
{
    /// <summary>
    /// Урожай, который вырастает из семян.
    /// </summary>
    public class CropWebModel : EntityBaseModel
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
        /// Семя, из которого вырастает этот урожай.
        /// </summary>
        public SeedWebModel Seed { get; set; }
    }
}
