using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Ингредиент алкоголя.
    /// </summary>
    public class AlcoholIngredient : EntityBase
    {
        /// <summary>
        /// Id алкоголя.
        /// </summary>
        public long AlcoholId { get; set; }

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
        /// Алкоголь.
        /// </summary>
        public virtual Alcohol Alcohol { get; set; }
    }
}
