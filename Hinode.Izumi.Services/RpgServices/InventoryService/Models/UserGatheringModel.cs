using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.InventoryService.Models
{
    /// <summary>
    /// Собирательский ресурс у пользователя.
    /// </summary>
    public class UserGatheringModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id собирательского ресурса.
        /// </summary>
        public long GatheringId { get; set; }

        /// <summary>
        /// Количество собирательского ресурса у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Название собирательского ресурса.
        /// </summary>
        public string Name { get; set; }
    }
}
