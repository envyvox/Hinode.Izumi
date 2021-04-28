using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.InventoryService.Models
{
    /// <summary>
    /// Валюта пользователя.
    /// </summary>
    public class UserCurrencyModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Количество валюты у пользователя.
        /// </summary>
        public long Amount { get; set; }
    }
}
