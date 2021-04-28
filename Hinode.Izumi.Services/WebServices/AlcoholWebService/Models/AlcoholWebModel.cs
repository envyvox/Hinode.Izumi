using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.AlcoholWebService.Models
{
    /// <summary>
    /// Алкоголь.
    /// </summary>
    public class AlcoholWebModel : EntityBaseModel
    {
        /// <summary>
        /// Название алкоголя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Длительность изготовления.
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Себестоимость изготовления алкоголя.
        /// </summary>
        public long CostPrice { get; set; }

        /// <summary>
        /// Стоимость изготовления.
        /// </summary>
        public long CraftingPrice { get; set; }

        /// <summary>
        /// Цена НПС.
        /// </summary>
        public long NpcPrice { get; set; }

        /// <summary>
        /// Чистая прибыль.
        /// </summary>
        public long Profit { get; set; }
    }
}
