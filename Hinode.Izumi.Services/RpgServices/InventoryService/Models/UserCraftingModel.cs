using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.InventoryService.Models
{
    /// <summary>
    /// Изготавливаемый предмет у пользователя.
    /// </summary>
    public class UserCraftingModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id изготавливаемого предмета.
        /// </summary>
        public long CraftingId { get; set; }

        /// <summary>
        /// Количество изготавливаемого предмета у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Название изготавливаемого предмета.
        /// </summary>
        public string Name { get; set; }
    }
}
