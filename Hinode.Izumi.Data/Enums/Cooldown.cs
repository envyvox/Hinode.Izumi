using System;

namespace Hinode.Izumi.Data.Enums
{
    public enum Cooldown
    {
        UpdateAbout = 1,
        GamblingBet = 2,
    }

    public static class CooldownHelper
    {
        /// <summary>
        /// Возвращает локализированное название кулдауна.
        /// </summary>
        /// <param name="cooldown">Кулдаун.</param>
        /// <returns>Локализированное название кулдауна.</returns>
        public static string Localize(this Cooldown cooldown) => cooldown switch
        {
            Cooldown.UpdateAbout => "Обновление информации",
            Cooldown.GamblingBet => "Ставка в казино",
            _ => throw new ArgumentOutOfRangeException(nameof(cooldown), cooldown, null)
        };
    }
}
