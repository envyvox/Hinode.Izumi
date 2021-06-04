using System;

namespace Hinode.Izumi.Data.Enums
{
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
        public static string Localize(this Season season) => season switch
        {
            Season.Any => "Любой",
            Season.Spring => "Весна",
            Season.Summer => "Лето",
            Season.Autumn => "Осень",
            Season.Winter => "Зима",
            _ => throw new ArgumentOutOfRangeException(nameof(season), season, null)
        };

        public static Image Image(this Season season) => season switch
        {
            Season.Spring => Enums.Image.Spring,
            Season.Summer => Enums.Image.Summer,
            Season.Autumn => Enums.Image.Autumn,
            Season.Winter => Enums.Image.Winter,
            _ => throw new ArgumentOutOfRangeException(nameof(season), season, null)
        };
    }
}
