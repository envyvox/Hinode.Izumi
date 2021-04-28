using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.IngredientService.Models
{
    /// <summary>
    /// Ингредиент блюда.
    /// </summary>
    public class FoodIngredientModel : EntityBaseModel
    {
        /// <summary>
        /// Id блюда.
        /// </summary>
        public long FoodId { get; set; }

        /// <summary>
        /// Категория ингредиента.
        /// </summary>
        public IngredientCategory Category { get; set; }

        /// <summary>
        /// Id ингредиента.
        /// </summary>
        public long IngredientId { get; set; }

        /// <summary>
        /// Количество ингредиентов необходимых для приготовления.
        /// </summary>
        public long Amount { get; set; }
    }
}
