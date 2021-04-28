using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.InventoryService.Models
{
    /// <summary>
    /// Блюдо у пользователя.
    /// </summary>
    public class UserFoodModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id блюда.
        /// </summary>
        public long FoodId { get; set; }

        /// <summary>
        /// Количество блюда у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Название блюда.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Необходимое мастерство для приготовления этого блюда.
        /// </summary>
        public long Mastery { get; set; }
    }
}
