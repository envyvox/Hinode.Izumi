using System.Collections.Generic;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.FoodWebService.Models
{
    /// <summary>
    /// Блюдо.
    /// </summary>
    public class FoodWebModel : EntityBaseModel
    {
        /// <summary>
        /// Id иконки.
        /// </summary>
        public string EmoteId { get; set; }

        /// <summary>
        /// Название блюда.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Необходимое мастерство для приготовления этого блюда.
        /// </summary>
        public long Mastery { get; set; }

        /// <summary>
        /// Длительность приготовления одной единицы этого блюда.
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Восстанавливаемая энергия при поедании этого блюда.
        /// </summary>
        public long Energy { get; set; }

        /// <summary>
        /// Себестоимость приготовления блюда.
        /// </summary>
        public long CostPrice { get; set; }

        /// <summary>
        /// Стоимость приготовления.
        /// </summary>
        public long CookingPrice { get; set; }

        /// <summary>
        /// Цена НПС.
        /// </summary>
        public long NpcPrice { get; set; }

        /// <summary>
        /// Чистая прибыль.
        /// </summary>
        public long Profit { get; set; }

        /// <summary>
        /// Стоимость рецепта.
        /// </summary>
        public long RecipePrice { get; set; }

        /// <summary>
        /// Продается ли рецепт этого блюда в магазине рецептов?
        /// </summary>
        public bool RecipeSellable { get; set; }

        /// <summary>
        /// Это особое блюдо события?
        /// </summary>
        public bool Event { get; set; }

        /// <summary>
        /// Сезоны блюда.
        /// </summary>
        public List<Season> Seasons { get; set; }
    }
}
