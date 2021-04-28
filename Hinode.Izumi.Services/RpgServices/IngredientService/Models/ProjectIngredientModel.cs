using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.IngredientService.Models
{
    /// <summary>
    /// Ингредиент для строительства.
    /// </summary>
    public class ProjectIngredientModel : EntityBaseModel
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
    }
}
