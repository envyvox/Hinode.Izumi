using System;
using System.Collections.Generic;
using System.Linq;

namespace Hinode.Izumi.Data.Enums.PropertyEnums
{
    /// <summary>
    /// Категория свойств игрового мира.
    /// </summary>
    public enum PropertyCategory
    {
        /// <summary>
        /// Потребление энергии.
        /// </summary>
        EnergyCost = 1,

        /// <summary>
        /// Длительность действия.
        /// </summary>
        ActionTime = 2,

        /// <summary>
        /// Уменьшение длительности действия.
        /// </summary>
        ActionTimeReduce = 3,

        /// <summary>
        /// Состояние мира.
        /// </summary>
        WorldState = 4,

        /// <summary>
        /// Экономика.
        /// </summary>
        Economy = 5,

        /// <summary>
        /// Перезарядка.
        /// </summary>
        Cooldown = 6,

        /// <summary>
        /// Строительство.
        /// </summary>
        Building = 7,

        /// <summary>
        /// Семья.
        /// </summary>
        Family = 8,

        /// <summary>
        /// Ежедневный босс.
        /// </summary>
        Boss = 9,

        /// <summary>
        /// Событие.
        /// </summary>
        Event = 10,

        /// <summary>
        /// Коробки.
        /// </summary>
        Box = 11
    }

    public static class PropertyCategoryHelper
    {
        /// <summary>
        /// Возвращает массив свойств которые входят в эту категорию.
        /// </summary>
        /// <param name="category">Категория свойств.</param>
        /// <returns>Массив свойств.</returns>
        public static IEnumerable<Property> Properties(this PropertyCategory category) =>
            Enum.GetValues(typeof(Property))
                .Cast<Property>()
                .Where(x => x.Category() == category);
    }
}
