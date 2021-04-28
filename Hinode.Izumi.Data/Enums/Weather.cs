using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Погода в игровом мире.
    /// </summary>
    public enum Weather
    {
        /// <summary>
        /// Любая погода.
        /// </summary>
        Any = 0,

        /// <summary>
        /// Ясная погода.
        /// </summary>
        Clear = 1,

        /// <summary>
        /// Дождливая погода.
        /// </summary>
        Rain = 2
    }

    public static class WeatherHelper
    {
        /// <summary>
        /// Возвращает локализированное название погоды.
        /// </summary>
        /// <param name="weather">Погода.</param>
        /// <returns>Локализированное название погоды.</returns>
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
