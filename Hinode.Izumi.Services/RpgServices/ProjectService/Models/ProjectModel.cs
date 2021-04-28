using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.ProjectService.Models
{
    /// <summary>
    /// Чертеж.
    /// </summary>
    public class ProjectModel : EntityBaseModel
    {
        /// <summary>
        /// Название чертежа.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Стоимость чертежа.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Длительность строительства (в часах).
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Id требуемой постройки (не обязательно).
        /// </summary>
        public long? ReqBuildingId { get; set; }
    }
}
