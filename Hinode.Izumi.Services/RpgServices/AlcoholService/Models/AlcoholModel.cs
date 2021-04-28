using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.AlcoholService.Models
{
    /// <summary>
    /// Алкоголь.
    /// </summary>
    public class AlcoholModel : EntityBaseModel
    {
        /// <summary>
        /// Название алкоголя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Длительность изготовления.
        /// </summary>
        public long Time { get; set; }
    }
}
