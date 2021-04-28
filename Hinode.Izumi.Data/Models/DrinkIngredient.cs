using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Ингредиент напитка.
    /// </summary>
    public class DrinkIngredient : EntityBase
    {
        /// <summary>
        /// Id напитка.
        /// </summary>
        public long DrinkId { get; set; }

        /// <summary>
        /// Категория ингредиента.
        /// </summary>
        public IngredientCategory Category { get; set; }

        /// <summary>
        /// Id ингредиента.
        /// </summary>
        public long IngredientId { get; set; }

        /// <summary>
        /// Количестов необходимых ингредиентов для изготовления.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Напиток.
        /// </summary>
        public virtual Drink Drink { get; set; }
    }
}
