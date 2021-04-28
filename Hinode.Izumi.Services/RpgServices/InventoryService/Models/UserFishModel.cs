using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.InventoryService.Models
{
    /// <summary>
    /// Рыба у пользователя.
    /// </summary>
    public class UserFishModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id рыбы.
        /// </summary>
        public long FishId { get; set; }

        /// <summary>
        /// Количество рыбы у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Название рыбы.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Редкость рыбы.
        /// </summary>
        public FishRarity Rarity { get; set; }

        /// <summary>
        /// Сезоны, в которые ее можно поймать и продать.
        /// </summary>
        public Season[] Seasons { get; set; }

        /// <summary>
        /// Цена рыбы.
        /// </summary>
        public long Price { get; set; }
    }
}
