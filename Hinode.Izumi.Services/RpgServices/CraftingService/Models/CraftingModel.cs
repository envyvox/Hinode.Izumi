using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.CraftingService.Models
{
    /// <summary>
    /// Изготавливаемый предмет.
    /// </summary>
    public class CraftingModel : EntityBaseModel
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
    }
}
