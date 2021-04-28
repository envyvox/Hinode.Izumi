using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.InventoryService.Models
{
    /// <summary>
    /// Семя у пользователя.
    /// </summary>
    public class UserSeedModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id семян.
        /// </summary>
        public long SeedId { get; set; }

        /// <summary>
        /// Количество семян у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Название семени.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Сезон семени.
        /// </summary>
        public Season Season { get; set; }
    }
}
