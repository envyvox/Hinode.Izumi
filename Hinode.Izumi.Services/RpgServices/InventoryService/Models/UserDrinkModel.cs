using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.InventoryService.Models
{
    /// <summary>
    /// Напиток у пользователя.
    /// </summary>
    public class UserDrinkModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id напитка.
        /// </summary>
        public long DrinkId { get; set; }

        /// <summary>
        /// Количество напитка у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Название напитка.
        /// </summary>
        public string Name { get; set; }
    }
}
