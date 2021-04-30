using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.FoodService.Models
{
    /// <summary>
    /// Блюдо.
    /// </summary>
    public class FoodModel : EntityBaseModel
    {
        /// <summary>
        /// Название блюда.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Необходимое мастерство для приготовления этого блюда.
        /// </summary>
        public long Mastery { get; set; }

        /// <summary>
        /// Длительность приготовления одной единицы этого блюда (в секундах).
        /// </summary>
        public long Time { get; set; }
    }
}
