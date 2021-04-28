using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Время суток.
    /// </summary>
    public enum TimesDay
    {
        Any = 0,
        Day = 1,
        Night = 2
    }

    public static class TimesDayHelper
    {
        /// <summary>
        /// Возвращает локализированное название времени суток.
        /// </summary>
        /// <param name="timesDay">Время суток.</param>
        /// <returns>Локализированное название времени суток.</returns>
        public static string Localize(this TimesDay timesDay) => timesDay switch
        {
            TimesDay.Any => "любое",
            TimesDay.Day => "день",
            TimesDay.Night => "ночь",
            _ => throw new ArgumentOutOfRangeException(nameof(timesDay), timesDay, null)
        };
    }
}
