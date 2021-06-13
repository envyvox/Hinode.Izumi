using System;
using System.Collections.Generic;
using System.Linq;
using Hinode.Izumi.Framework.Extensions;

namespace Hinode.Izumi.Data.Enums.ReputationEnums
{
    public enum ReputationStatus : byte
    {
        Neutral = 0,
        Dear = 1,
        Virtuous = 2,
        Honorable = 3,
        Illustrious = 4,
        Godlike = 5
    }

    public static class ReputationStatusHelper
    {
        private static readonly Dictionary<long, ReputationStatus> ReputationStatusBrackets = new()
        {
            {0, ReputationStatus.Neutral},
            {500, ReputationStatus.Dear},
            {1000, ReputationStatus.Virtuous},
            {2000, ReputationStatus.Honorable},
            {5000, ReputationStatus.Illustrious},
            {10000, ReputationStatus.Godlike}
        };

        private static readonly Dictionary<long, long> ReputationStarsBrackets = new()
        {
            {0, 0}, {500, 1}, {1000, 2}, {2000, 3}, {5000, 4}, {10000, 5}
        };

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

        public static ReputationStatus GetReputationStatus(double averageReputation) =>
            ReputationStatusBrackets[ReputationStatusBrackets.Keys.Where(x => x <= averageReputation).Max()];

        public static string Emote(double amount) =>
            "ReputationStatus" +
            // определяем количество звезд
            ReputationStarsBrackets.MaxValue((long) amount) +
            // добавляем последнюю букву названия иконки
            "S";
    }
}
