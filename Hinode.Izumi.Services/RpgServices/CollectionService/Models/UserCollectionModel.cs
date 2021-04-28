using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.CollectionService.Models
{
    /// <summary>
    /// Коллекция пользователя.
    /// </summary>
    public class UserCollectionModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Категория коллекции.
        /// </summary>
        public CollectionCategory Category { get; set; }

        /// <summary>
        /// Id предмета.
        /// </summary>
        public long ItemId { get; set; }
    }
}
