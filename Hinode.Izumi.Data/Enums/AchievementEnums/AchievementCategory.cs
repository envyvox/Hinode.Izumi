using System;
using System.Collections.Generic;
using System.Linq;

namespace Hinode.Izumi.Data.Enums.AchievementEnums
{
    /// <summary>
    /// Категория достижения.
    /// </summary>
    public enum AchievementCategory
    {
        FirstSteps = 1,
        Gathering = 2,
        Fishing = 3,
        Harvesting = 4,
        Cooking = 5,
        Crafting = 6,
        Trading = 7,
        Alchemy = 8,
        Casino = 9,
        Collection = 10,
        Event = 11
    }

    public static class AchievementCategoryHelper
    {
        /// <summary>
        /// Возвращает локализированное название категории достижений.
        /// </summary>
        /// <param name="category">Категория достижений.</param>
        /// <returns>Локализированное название категории достижений.</returns>
        public static string Localize(this AchievementCategory category) => category switch
        {
            AchievementCategory.FirstSteps => "Первые шаги",
            AchievementCategory.Gathering => "Собирательство",
            AchievementCategory.Fishing => "Рыбалка",
            AchievementCategory.Harvesting => "Урожай",
            AchievementCategory.Cooking => "Кулинария",
            AchievementCategory.Crafting => "Изготовление",
            AchievementCategory.Trading => "Торговля",
            AchievementCategory.Alchemy => "Алхимия",
            AchievementCategory.Casino => "Казино",
            AchievementCategory.Collection => "Коллекция",
            AchievementCategory.Event => "События",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };

        /// <summary>
        /// Возвращает массив достижений которые входят в категорию.
        /// </summary>
        /// <param name="category">Категория достижений.</param>
        /// <returns>Массив достижений которые входят в категорию.</returns>
        public static IEnumerable<Achievement> Achievements(this AchievementCategory category) =>
            Enum.GetValues(typeof(Achievement))
                .Cast<Achievement>()
                .Where(x => x.Category() == category);
    }
}
