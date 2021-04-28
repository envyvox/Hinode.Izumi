using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.InventoryService.Models
{
    /// <summary>
    /// Урожай у пользователя.
    /// </summary>
    public class UserCropModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id урожая.
        /// </summary>
        public long CropId { get; set; }

        /// <summary>
        /// Количество урожая у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Название урожая.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Сезон урожая.
        /// </summary>
        public Season Season { get; set; }
    }
}
