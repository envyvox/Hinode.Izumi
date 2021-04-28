using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Категория инвентаря.
    /// </summary>
    public enum InventoryCategory
    {
        Currency = 1,
        Box = 11,
        Gathering = 2,
        Product = 3,
        Crafting = 4,
        Alcohol = 5,
        Drink = 6,
        Seed = 7,
        Crop = 8,
        Fish = 9,
        Food = 10
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
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
