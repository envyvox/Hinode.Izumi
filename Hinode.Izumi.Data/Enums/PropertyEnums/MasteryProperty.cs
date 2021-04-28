using System;

namespace Hinode.Izumi.Data.Enums.PropertyEnums
{
    /// <summary>
    /// Свойство (настройка) мастерства.
    /// </summary>
    public enum MasteryProperty
    {
        /// <summary>
        /// Длительность исследования.
        /// </summary>
        ActionTimeGathering = 1,

        /// <summary>
        /// Шанс срыва обычной рыбы.
        /// </summary>
        FishFailChanceCommon = 2,

        /// <summary>
        /// Шанс срыва редкой рыбы.
        /// </summary>
        FishFailChanceRare = 3,

        /// <summary>
        /// Шанс срыва эпической рыбы.
        /// </summary>
        FishFailChanceEpic = 4,

        /// <summary>
        /// Шанс срыва мифической рыбы.
        /// </summary>
        FishFailChanceMythical = 5,

        /// <summary>
        /// Шанс срыва легендарной рыбы.
        /// </summary>
        FishFailChanceLegendary = 6,

        /// <summary>
        /// Шанс срыва божественной рыбы.
        /// </summary>
        FishFailChanceDivine = 7,

        /// <summary>
        /// Шанс выловить обычную рыбу.
        /// </summary>
        FishRarityChanceCommon = 8,

        /// <summary>
        /// Шанс выловить редкую рыбу.
        /// </summary>
        FishRarityChanceRare = 9,

        /// <summary>
        /// Шанс выловить эпическую рыбу.
        /// </summary>
        FishRarityChanceEpic = 10,

        /// <summary>
        /// Шанс выловить мифическую рыбу.
        /// </summary>
        FishRarityChanceMythical = 11,

        /// <summary>
        /// Шанс выловить легендарную рыбу.
        /// </summary>
        FishRarityChanceLegendary = 12,

        /// <summary>
        /// Шанс выловить божественную рыбу.
        /// </summary>
        FishRarityChanceDivine = 13,

        /// <summary>
        /// Скидка на семена.
        /// </summary>
        TradingMasterySeedDiscount = 14,

        /// <summary>
        /// Скидка на оплату транспорта.
        /// </summary>
        TradingMasteryTransitDiscount = 15,

        /// <summary>
        /// Скидка на особые предложения.
        /// </summary>
        TradingMasterySpecialOfferDiscount = 16,

        /// <summary>
        /// Налог с рынка.
        /// </summary>
        TradingMasteryMarketTax = 17
    }

    public static class MasteryPropertyHelper
    {
        /// <summary>
        /// Возвращает категорию к которой относится это свойство.
        /// </summary>
        /// <param name="property">Свойство.</param>
        /// <returns>Категория.</returns>
        public static MasteryPropertyCategory Category(this MasteryProperty property) => property switch
        {
            MasteryProperty.ActionTimeGathering => MasteryPropertyCategory.ActionTime,
            MasteryProperty.FishFailChanceCommon => MasteryPropertyCategory.FishFailChance,
            MasteryProperty.FishFailChanceRare => MasteryPropertyCategory.FishFailChance,
            MasteryProperty.FishFailChanceEpic => MasteryPropertyCategory.FishFailChance,
            MasteryProperty.FishFailChanceMythical => MasteryPropertyCategory.FishFailChance,
            MasteryProperty.FishFailChanceLegendary => MasteryPropertyCategory.FishFailChance,
            MasteryProperty.FishFailChanceDivine => MasteryPropertyCategory.FishFailChance,
            MasteryProperty.FishRarityChanceCommon => MasteryPropertyCategory.FishRarityChance,
            MasteryProperty.FishRarityChanceRare => MasteryPropertyCategory.FishRarityChance,
            MasteryProperty.FishRarityChanceEpic => MasteryPropertyCategory.FishRarityChance,
            MasteryProperty.FishRarityChanceMythical => MasteryPropertyCategory.FishRarityChance,
            MasteryProperty.FishRarityChanceLegendary => MasteryPropertyCategory.FishRarityChance,
            MasteryProperty.FishRarityChanceDivine => MasteryPropertyCategory.FishRarityChance,
            MasteryProperty.TradingMasterySeedDiscount => MasteryPropertyCategory.TradingMastery,
            MasteryProperty.TradingMasteryTransitDiscount => MasteryPropertyCategory.TradingMastery,
            MasteryProperty.TradingMasterySpecialOfferDiscount => MasteryPropertyCategory.TradingMastery,
            MasteryProperty.TradingMasteryMarketTax => MasteryPropertyCategory.TradingMastery,
            _ => throw new ArgumentOutOfRangeException(nameof(property), property, null)
        };
    }
}
