using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.IngredientService.Models
{
    /// <summary>
    /// Ингредиент напитка.
    /// </summary>
    public class DrinkIngredientModel : EntityBaseModel
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
    }
}
