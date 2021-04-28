using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.IngredientService.Models
{
    /// <summary>
    /// Ингредиент алкоголя.
    /// </summary>
    public class AlcoholIngredientModel : EntityBaseModel
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
    }
}
