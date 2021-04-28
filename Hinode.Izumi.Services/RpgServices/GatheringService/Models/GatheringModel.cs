using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.GatheringService.Models
{
    /// <summary>
    /// Собирательский ресурс.
    /// </summary>
    public class GatheringModel : EntityBaseModel
    {
        /// <summary>
        /// Название собирательского ресурса.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Цена собирательского ресурса.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Локация, в которой его можно получить.
        /// </summary>
        public Location Location { get; set; }
    }
}
