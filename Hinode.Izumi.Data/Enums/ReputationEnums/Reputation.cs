using System;
using System.Collections.Generic;
using Hinode.Izumi.Framework.Extensions;

namespace Hinode.Izumi.Data.Enums.ReputationEnums
{
    public enum Reputation : byte
    {
        Capital = 1,
        Garden = 2,
        Seaport = 3,
        Castle = 4,
        Village = 5
    }

    public static class ReputationHelper
    {
        private static readonly Dictionary<long, long> ReputationStarsBrackets = new()
        {
            {0, 0}, {500, 1}, {1000, 2}, {2000, 3}, {5000, 4}, {10000, 5}
        };

        public static Location Location(this Reputation reputation) => reputation switch
        {
            Reputation.Capital => Enums.Location.Capital,
            Reputation.Garden => Enums.Location.Garden,
            Reputation.Seaport => Enums.Location.Seaport,
            Reputation.Castle => Enums.Location.Castle,
            Reputation.Village => Enums.Location.Village,
            _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
        };

        public static string Emote(this Reputation reputation, long amount) =>
            // получаем название репутации
            "Reputation" + reputation.Location() +
            // определеяем количество звезд
            ReputationStarsBrackets.MaxValue(amount) +
            // добавляем последнюю букву названия иконки
            "S";
    }
}
