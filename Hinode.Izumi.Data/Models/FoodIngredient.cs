using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Ингредиент блюда.
    /// </summary>
    public class FoodIngredient : EntityBase
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

        /// <summary>
        /// Блюдо.
        /// </summary>
        public virtual Food Food { get; set; }
    }
}
