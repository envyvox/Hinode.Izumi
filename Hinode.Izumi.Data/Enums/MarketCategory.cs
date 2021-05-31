using System;

namespace Hinode.Izumi.Data.Enums
{
    public enum MarketCategory
    {
        Gathering = 1,
        Crafting = 2,
        Alcohol = 3,
        Drink = 4,
        Food = 5,
        Crop = 6
    }

    public static class MarketCategoryHelper
    {
        public static string Localize(this MarketCategory category) => category switch
        {
            MarketCategory.Gathering => InventoryCategory.Gathering.Localize(),
            MarketCategory.Crafting => InventoryCategory.Crafting.Localize(),
            MarketCategory.Alcohol => InventoryCategory.Alcohol.Localize(),
            MarketCategory.Drink => InventoryCategory.Drink.Localize(),
            MarketCategory.Food => InventoryCategory.Food.Localize(),
            MarketCategory.Crop => InventoryCategory.Crop.Localize(),
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
