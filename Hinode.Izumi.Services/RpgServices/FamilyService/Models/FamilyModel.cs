using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.FamilyService.Models
{
    /// <summary>
    /// Семья.
    /// </summary>
    public class FamilyModel : EntityBaseModel
    {
        /// <summary>
        /// Статус семьи.
        /// </summary>
        public FamilyStatus Status { get; set; }

        /// <summary>
        /// Название семьи.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание семьи.
        /// </summary>
        public string Description { get; set; }
    }
}
