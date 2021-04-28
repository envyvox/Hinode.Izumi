using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.InventoryService.Models
{
    /// <summary>
    /// Алкоголь у пользователя.
    /// </summary>
    public class UserAlcoholModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id алкоголя.
        /// </summary>
        public long AlcoholId { get; set; }

        /// <summary>
        /// Количество алкоголя у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Название алкоголя.
        /// </summary>
        public string Name { get; set; }
    }
}
