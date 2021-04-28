using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.InventoryService.Models
{
    /// <summary>
    /// Продукт у пользователя.
    /// </summary>
    public class UserProductModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id продукта.
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// Количество продукта у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Название продукта.
        /// </summary>
        public string Name { get; set; }
    }
}
