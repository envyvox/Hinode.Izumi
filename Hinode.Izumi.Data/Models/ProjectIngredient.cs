using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Ингредиент для строительства.
    /// </summary>
    public class ProjectIngredient : EntityBase
    {
        /// <summary>
        /// Id чертежа.
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// Категория ингредиента.
        /// </summary>
        public IngredientCategory Category { get; set; }

        /// <summary>
        /// Id ингредиента.
        /// </summary>
        public long IngredientId { get; set; }

        /// <summary>
        /// Количество ингредиентов необходимых для строительства.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Чертеж.
        /// </summary>
        public virtual Project Project { get; set; }
    }
}
