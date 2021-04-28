using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Категория предмета на рынке.
    /// </summary>
    public enum MarketCategory
    {
        Gathering = 1,
        Crafting = 2,
        Alcohol = 3,
        Drink = 4,
        Food = 5
    }

    public static class MarketCategoryHelper
    {
        /// <summary>
        /// Возвращает локализированное название категории предмета на рынке.
        /// </summary>
        /// <param name="category">Категория предмета на рынке.</param>
        /// <returns>Локализированное название категории предмета на рынке.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Localize(this MarketCategory category) => category switch
        {
            MarketCategory.Gathering => InventoryCategory.Gathering.Localize(),
            MarketCategory.Crafting => InventoryCategory.Crafting.Localize(),
            MarketCategory.Alcohol => InventoryCategory.Alcohol.Localize(),
            MarketCategory.Drink => InventoryCategory.Drink.Localize(),
            MarketCategory.Food => InventoryCategory.Food.Localize(),
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
