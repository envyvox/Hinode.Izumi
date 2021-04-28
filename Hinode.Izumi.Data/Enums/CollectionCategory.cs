using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Категория коллекции.
    /// </summary>
    public enum CollectionCategory
    {
        Gathering = 1,
        Crafting = 2,
        Alcohol = 3,
        Drink = 4,
        Crop = 5,
        Fish = 6,
        Food = 7
    }

    public static class CollectionCategoryHelper
    {
        /// <summary>
        /// Возвращает локализированное название категории коллекции.
        /// </summary>
        /// <param name="category">Категория коллекции.</param>
        /// <returns>Локализированное название категории коллекции.</returns>
        public static string Localize(this CollectionCategory category) => category switch
        {
            CollectionCategory.Gathering => "Собительские предметы",
            CollectionCategory.Crafting => "Изготавливаемые предметы",
            CollectionCategory.Alcohol => "Алкоголь",
            CollectionCategory.Drink => "Напитки",
            CollectionCategory.Crop => "Урожай",
            CollectionCategory.Fish => "Рыба",
            CollectionCategory.Food => "Блюда",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
