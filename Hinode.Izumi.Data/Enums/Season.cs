using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Сезон игрового мире.
    /// </summary>
    public enum Season
    {
        Any = 0,
        Spring = 1,
        Summer = 2,
        Autumn = 3,
        Winter = 4
    }

    public static class SeasonHelper
    {
        /// <summary>
        /// Возвращает локализированное название сезона.
        /// </summary>
        /// <param name="season">Сезон.</param>
        /// <returns>Локализированное название сезона.</returns>
        public static string Localize(this Season season) => season switch
        {
            Season.Any => "Любой",
            Season.Spring => "Весна",
            Season.Summer => "Лето",
            Season.Autumn => "Осень",
            Season.Winter => "Зима",
            _ => throw new ArgumentOutOfRangeException(nameof(season), season, null)
        };
    }
}
