using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Ингредиент изготавливаемого предмета.
    /// </summary>
    public class CraftingIngredient : EntityBase
    {
        /// <summary>
        /// Id изготавливаемого предмета.
        /// </summary>
        public long CraftingId { get; set; }

        /// <summary>
        /// Категория ингредиента.
        /// </summary>
        public IngredientCategory Category { get; set; }

        /// <summary>
        /// Id ингредиента.
        /// </summary>
        public long IngredientId { get; set; }

        /// <summary>
        /// Количество ингредиентов необходимых для изготовления.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Изготавливаемый предмет.
        /// </summary>
        public virtual Crafting Crafting { get; set; }
    }
}
