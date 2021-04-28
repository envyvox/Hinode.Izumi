using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.CraftingWebService.Models
{
    /// <summary>
    /// Изготовленный предмет.
    /// </summary>
    public class CraftingWebModel : EntityBaseModel
    {
        /// <summary>
        /// Название изготавливаемого предмета.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Длительность изготовления этого предмета.
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Локация, в которой можно изготовить этот предмет.
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Себестоимость изготовления предмета.
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
