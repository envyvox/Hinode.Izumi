using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Категория инвентаря.
    /// </summary>
    public enum InventoryCategory
    {
        Currency = 1,
        Box = 2,
        Gathering = 3,
        Product = 4,
        Crafting = 5,
        Alcohol = 6,
        Drink = 7,
        Seed = 8,
        Crop = 9,
        Fish = 10,
        Food = 11,
        Seafood = 12
    }

    public static class InventoryCategoryHelper
    {
        /// <summary>
        /// Возвращает локализированное название категории инвентаря.
        /// </summary>
        /// <param name="category">Категория инвенторя.</param>
        /// <returns>Локализированное название категории инвентаря.</returns>
        public static string Localize(this InventoryCategory category) => category switch
        {
            InventoryCategory.Currency => "Валюта",
            InventoryCategory.Gathering => "Собирательские ресурсы",
            InventoryCategory.Product => "Продукты",
            InventoryCategory.Crafting => "Изготавливаемые предметы",
            InventoryCategory.Alcohol => "Алкоголь",
            InventoryCategory.Drink => "Напитки",
            InventoryCategory.Seed => "Семена",
            InventoryCategory.Crop => "Урожай",
            InventoryCategory.Fish => "Рыба",
            InventoryCategory.Food => "Блюда",
            InventoryCategory.Box => "Коробки",
            InventoryCategory.Seafood => "Морепродукты",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
