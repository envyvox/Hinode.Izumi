using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.CraftingIngredientWebService.Models
{
    /// <summary>
    /// Ингредиент изготавливаемого предмета.
    /// </summary>
    public class CraftingIngredientWebModel : EntityBaseModel
    {
        /// <summary>
        /// Id изготавливаемого предмета.
        /// </summary>
        public long CraftingId { get; set; }

        /// <summary>
        /// Название изготавливаемого предмета.
        /// </summary>
        public string CraftingName { get; set; }

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
        /// Количество ингредиентов необходимых для изготовления.
        /// </summary>
        public long Amount { get; set; }
    }
}
