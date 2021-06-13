using System;

namespace Hinode.Izumi.Data.Enums.PropertyEnums
{
    public enum MasteryProperty : byte
    {
        ActionTimeGathering = 1,
        FishFailChanceCommon = 2,
        FishFailChanceRare = 3,
        FishFailChanceEpic = 4,
        FishFailChanceMythical = 5,
        FishFailChanceLegendary = 6,
        FishFailChanceDivine = 7,
        FishRarityChanceCommon = 8,
        FishRarityChanceRare = 9,
        FishRarityChanceEpic = 10,
        FishRarityChanceMythical = 11,
        FishRarityChanceLegendary = 12,
        FishRarityChanceDivine = 13,
        TradingMasterySeedDiscount = 14,
        TradingMasteryTransitDiscount = 15,
        TradingMasterySpecialOfferDiscount = 16,
        TradingMasteryMarketTax = 17
    }

    public static class MasteryPropertyHelper
    {
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
