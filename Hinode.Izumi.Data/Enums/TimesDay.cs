using System;

namespace Hinode.Izumi.Data.Enums
{
    public enum TimesDay : byte
    {
        Any = 0,
        Day = 1,
        Night = 2
    }

    public static class TimesDayHelper
    {
        public static string Localize(this TimesDay timesDay) => timesDay switch
        {
            TimesDay.Any => "любое",
            TimesDay.Day => "день",
            TimesDay.Night => "ночь",
            _ => throw new ArgumentOutOfRangeException(nameof(timesDay), timesDay, null)
        };
    }
}
