using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.InventoryService.Models
{
    /// <summary>
    /// Коробка у пользователя.
    /// </summary>
    public class UserBoxModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Коробка.
        /// </summary>
        public Box Box { get; set; }

        /// <summary>
        /// Количество коробок у пользователя.
        /// </summary>
        public long Amount { get; set; }
    }
}
