using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Категория постройки.
    /// </summary>
    public enum BuildingCategory
    {
        Personal = 1,
        Family = 2,
        Clan = 3
    }

    public static class BuildingCategoryHelper
    {
        /// <summary>
        /// Возвращает локализированное название категории постройки.
        /// </summary>
        /// <param name="category">Категория постройки.</param>
        /// <param name="declension">Склонить "какой"?</param>
        /// <returns>Локализированное название категории постройки.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Localize(this BuildingCategory category, bool declension = false) => category switch
        {
            BuildingCategory.Personal => declension ? "Персональный" : "Персональные",
            BuildingCategory.Family => declension ? "Семейный" : "Семейные",
            BuildingCategory.Clan => declension ? "Клановый" : "Клановые",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
