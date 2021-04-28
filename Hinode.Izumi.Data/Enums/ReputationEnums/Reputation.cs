using System;
using System.Collections.Generic;
using Hinode.Izumi.Framework.Extensions;

namespace Hinode.Izumi.Data.Enums.ReputationEnums
{
    /// <summary>
    /// Репутация локации.
    /// </summary>
    public enum Reputation
    {
        Capital = 1,
        Garden = 2,
        Seaport = 3,
        Castle = 4,
        Village = 5
    }

    public static class ReputationHelper
    {
        /// <summary>
        /// Библиотека с количеством звезд репутации по брекетам репутации.
        /// </summary>
        private static readonly Dictionary<long, long> ReputationStarsBrackets = new()
        {
            {0, 0}, {500, 1}, {1000, 2}, {2000, 3}, {5000, 4}, {10000, 5}
        };

        /// <summary>
        /// Возвращает локацию в которой доступна эта репутация.
        /// </summary>
        /// <param name="reputation">Репутация.</param>
        /// <returns>Локация в которой доступна эта репутация.</returns>
        public static Location Location(this Reputation reputation) => reputation switch
        {
            Reputation.Capital => Enums.Location.Capital,
            Reputation.Garden => Enums.Location.Garden,
            Reputation.Seaport => Enums.Location.Seaport,
            Reputation.Castle => Enums.Location.Castle,
            Reputation.Village => Enums.Location.Village,
            _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
        };

        /// <summary>
        /// Возвращает название иконки репутации.
        /// </summary>
        /// <param name="reputation">Репутация.</param>
        /// <param name="amount">Количество репутации.</param>
        /// <returns>Название иконки репутации.</returns>
        public static string Emote(this Reputation reputation, long amount) =>
            "Reputation" + reputation.Location() +
            // определеяем количество звезд
            ReputationStarsBrackets.MaxValue(amount) +
            // добавляем последнюю букву названия иконки
            "S";
    }
}
