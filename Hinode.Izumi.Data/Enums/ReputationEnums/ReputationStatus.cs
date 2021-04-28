using System;
using System.Collections.Generic;
using System.Linq;
using Hinode.Izumi.Framework.Extensions;

namespace Hinode.Izumi.Data.Enums.ReputationEnums
{
    /// <summary>
    /// Репутационный статус.
    /// </summary>
    public enum ReputationStatus
    {
        /// <summary>
        /// Репутационный статус по-умолчанию, за 0 репутационного рейтинга.
        /// </summary>
        Neutral = 0,
        /// <summary>
        /// Репутационный статус за 500 репутационного рейтинга.
        /// </summary>
        Dear = 1,
        /// <summary>
        /// Репутационный статус за 1000 репутационного рейтинга.
        /// </summary>
        Virtuous = 2,
        /// <summary>
        /// Репутационный статус за 2000 репутационного рейтинга.
        /// </summary>
        Honorable = 3,
        /// <summary>
        /// Репутационный статус за 5000 репутационного рейтинга.
        /// </summary>
        Illustrious = 4,
        /// <summary>
        /// Репутационный статус за 10000 репутационного рейтинга.
        /// </summary>
        Godlike = 5,
    }

    public static class ReputationStatusHelper
    {
        /// <summary>
        /// Библиотека репутационного статуса по брекетам репутации.
        /// </summary>
        private static readonly Dictionary<long, ReputationStatus> ReputationStatusBrackets = new()
        {
            {0, ReputationStatus.Neutral},
            {500, ReputationStatus.Dear},
            {1000, ReputationStatus.Virtuous},
            {2000, ReputationStatus.Honorable},
            {5000, ReputationStatus.Illustrious},
            {10000, ReputationStatus.Godlike}
        };

        /// <summary>
        /// Библиотека с количеством звезд репутационного статуса по брекетам репутации.
        /// </summary>
        private static readonly Dictionary<long, long> ReputationStarsBrackets = new()
        {
            {0, 0}, {500, 1}, {1000, 2}, {2000, 3}, {5000, 4}, {10000, 5}
        };

        /// <summary>
        /// Возвращает мастимальное количество мастерства для этого репутационного статуса.
        /// </summary>
        /// <param name="status">Репутационный статус.</param>
        /// <returns>Мастимальное количество мастерства.</returns>
        public static double MaxMastery(this ReputationStatus status) => status switch
        {
            ReputationStatus.Neutral => 49.99,
            ReputationStatus.Dear => 99.99,
            ReputationStatus.Virtuous => 149.99,
            ReputationStatus.Honorable => 199.99,
            ReputationStatus.Illustrious => 249.99,
            ReputationStatus.Godlike => 299.99,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };

        /// <summary>
        /// Возвращает локализированное название репутационного статуса.
        /// </summary>
        /// <param name="status">Репутационный статус.</param>
        /// <returns>Локализированное название репутационного статуса.</returns>
        public static string Localize(this ReputationStatus status) => status switch
        {
            ReputationStatus.Neutral => "Нейтральный",
            ReputationStatus.Dear => "Уважаемый",
            ReputationStatus.Virtuous => "Добродетельный",
            ReputationStatus.Honorable => "Благородный",
            ReputationStatus.Illustrious => "Прославленный",
            ReputationStatus.Godlike => "Богоподобный",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };

        /// <summary>
        /// Возвращает репутационный статус по количеству репутацинного рейтина (среднее значение репутаций).
        /// </summary>
        /// <param name="averageReputation">Репутационный рейтинг (среднее значение репутаций).</param>
        /// <returns>Репутационный статус.</returns>
        public static ReputationStatus GetReputationStatus(double averageReputation) =>
            ReputationStatusBrackets[ReputationStatusBrackets.Keys.Where(x => x <= averageReputation).Max()];

        /// <summary>
        /// Возвращает название иконки репутационного статуса.
        /// </summary>
        /// <param name="amount">Репутационный рейтинг (среднее значение репутаций).</param>
        /// <returns>Название иконки репутационного статуса.</returns>
        public static string Emote(double amount) =>
            "ReputationStatus" +
            // определяем количество звезд
            ReputationStarsBrackets.MaxValue((long) amount) +
            // добавляем последнюю букву названия иконки
            "S";
    }
}
