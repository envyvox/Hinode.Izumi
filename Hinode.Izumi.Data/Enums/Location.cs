using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Локация в игровом мире.
    /// </summary>
    public enum Location
    {
        InTransit = 0,
        Capital = 1,
        Garden = 2,
        Seaport = 3,
        Castle = 4,
        Village = 5,
        ExploreGarden = 6,
        ExploreCastle = 8,
        Fishing = 9,
        CapitalCasino = 10,
        CapitalMarket = 11,
        CapitalShop = 12,
        FieldWatering = 13,
        WorkOnContract = 14,
        MakingCrafting = 15,
        MakingAlcohol = 16,
        MakingFood = 17,
        MakingDrink = 18
    }

    public static class LocationHelper
    {
        /// <summary>
        /// Проверяет локацию на ее тип (локация или подлокация).
        /// </summary>
        /// <param name="location">Локация.</param>
        /// <returns>True если локация это подлокация, false если нет.</returns>
        public static bool SubLocation(this Location location) => location switch
        {
            Location.Capital => false,
            Location.Garden => false,
            Location.Seaport => false,
            Location.Castle => false,
            Location.Village => false,
            _ => true
        };

        /// <summary>
        /// Возвращает локализированное название локации.
        /// </summary>
        /// <param name="location">Локация.</param>
        /// <param name="declension">Склонение (в локации)?</param>
        /// <returns>Локализированное название локации.</returns>
        public static string Localize(this Location location, bool declension = false) => location switch
        {
            Location.InTransit => declension ? "пути" : "В пути",
            Location.Capital => declension ? "столице «Эдо»" : "Столица «Эдо»",
            Location.Garden => declension ? "цветущем саду «Кайраку-эн»" : "Цветущий сад «Кайраку-эн»",
            Location.Seaport => declension ? "портовом городе «Нагоя»" : "Портовый город «Нагоя»",
            Location.Castle => declension ? "древнем замке «Химэдзи»" : "Древний замок «Химэдзи»",
            Location.Village => declension ? "деревне «Мура»" : "Деревня «Мура»",
            Location.ExploreGarden => declension ? "исследовании сада" : "Исследование сада",
            Location.ExploreCastle => declension ? "исследовании шахт" : "Исследование шахт",
            Location.Fishing => declension ? "рыбалке" : "Рыбалка",
            Location.CapitalCasino => declension ? "казино «Коун»" : "Казино «Коун»",
            Location.CapitalMarket => declension ? "рынке «Сайсё»" : "Рынок «Сайсё»",
            Location.CapitalShop => declension ? "магазинах «У Торедо»" : "Магазины «У Торедо»",
            Location.FieldWatering => declension ? "поливке участка земли" : "Поливка участка земли",
            Location.WorkOnContract => declension ? "." : "..", // Вместо названия локации выводится название контракта
            Location.MakingCrafting => declension ? "изготовлении предметов" : "Изготовление предметов",
            Location.MakingAlcohol => declension ? "изготовлении алкоголя" : "Изготовление алкоголя",
            Location.MakingFood => declension ? "приготовлении блюда" : "Приготовление блюда",
            Location.MakingDrink => declension ? "изготовлении напитков" : "Приготовление напитков",
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };
    }
}
