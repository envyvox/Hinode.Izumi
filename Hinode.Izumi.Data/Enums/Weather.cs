using System;

namespace Hinode.Izumi.Data.Enums
{
    public enum Weather
    {
        Any = 0,
        Clear = 1,
        Rain = 2
    }

    public static class WeatherHelper
    {
        public static string Localize(this Weather weather) => weather switch
        {
            // выводится в информации о мире, необходимо склонение "какой?"
            Weather.Any => "любой",
            Weather.Clear => "ясной",
            Weather.Rain => "дождливой",
            _ => throw new ArgumentOutOfRangeException(nameof(weather), weather, null)
        };
    }
}
