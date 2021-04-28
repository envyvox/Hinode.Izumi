using System;
using System.Collections.Generic;
using System.Linq;

namespace Hinode.Izumi.Data.Enums.PropertyEnums
{
    /// <summary>
    /// Категория свойства мастерства.
    /// </summary>
    public enum MasteryPropertyCategory
    {
        /// <summary>
        /// Длительность действия.
        /// </summary>
        ActionTime = 1,

        /// <summary>
        /// Шанс срыва рыбы.
        /// </summary>
        FishFailChance = 2,

        /// <summary>
        /// Шанс выловить рыбу этой редкости.
        /// </summary>
        FishRarityChance = 3,

        /// <summary>
        /// Бонусы мастерства торговли.
        /// </summary>
        TradingMastery = 4
    }

    public static class MasteryPropertyCategoryHelper
    {
        /// <summary>
        /// Возвращает массив свойств которые входят в эту категорию.
        /// </summary>
        /// <param name="category">Категория.</param>
        /// <returns>Массив свойств.</returns>
        public static IEnumerable<MasteryProperty> Properties(this MasteryPropertyCategory category) =>
            Enum.GetValues(typeof(MasteryProperty))
                .Cast<MasteryProperty>()
                .Where(x => x.Category() == category);
    }
}
