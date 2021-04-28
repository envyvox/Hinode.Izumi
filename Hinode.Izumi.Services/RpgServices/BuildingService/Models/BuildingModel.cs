using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.BuildingService.Models
{
    /// <summary>
    /// Постройка.
    /// </summary>
    public class BuildingModel : EntityBaseModel
    {
        /// <summary>
        /// Постройка.
        /// </summary>
        public Building Type { get; set; }

        /// <summary>
        /// Id чертежа (не обязательно).
        /// </summary>
        public long? ProjectId { get; set; }

        /// <summary>
        /// Категория постройки.
        /// </summary>
        public BuildingCategory Category { get; set; }

        /// <summary>
        /// Название постройки.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание постройки.
        /// </summary>
        public string Description { get; set; }
    }
}
