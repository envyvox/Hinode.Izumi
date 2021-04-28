using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.FoodIngredientWebService.Models
{
    /// <summary>
    /// Ингредиент блюда.
    /// </summary>
    public class FoodIngredientWebModel : EntityBaseModel
    {
        /// <summary>
        /// Id блюда.
        /// </summary>
        public long FoodId { get; set; }

        /// <summary>
        /// Название блюда.
        /// </summary>
        public string FoodName { get; set; }

        /// <summary>
        /// Категория ингредиента.
        /// </summary>
        public IngredientCategory Category { get; set; }

        /// <summary>
        /// Id ингредиента.
        /// </summary>
        public long IngredientId { get; set; }

        /// <summary>
        /// Название ингредиента.
        /// </summary>
        public string IngredientName { get; set; }

        /// <summary>
        /// Количество ингредиентов необходимых для приготовления.
        /// </summary>
        public long Amount { get; set; }
    }
}
