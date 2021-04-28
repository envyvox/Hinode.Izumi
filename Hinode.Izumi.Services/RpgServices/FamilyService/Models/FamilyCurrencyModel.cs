using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.FamilyService.Models
{
    /// <summary>
    /// Валюта семьи.
    /// </summary>
    public class FamilyCurrencyModel : EntityBaseModel
    {
        /// <summary>
        /// Id семьи.
        /// </summary>
        public long FamilyId { get; set; }

        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Количество валюты у семьи.
        /// </summary>
        public long Amount { get; set; }
    }
}
