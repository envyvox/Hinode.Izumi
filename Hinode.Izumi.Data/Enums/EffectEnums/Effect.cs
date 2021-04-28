using System;

namespace Hinode.Izumi.Data.Enums.EffectEnums
{
    /// <summary>
    /// Эффект.
    /// </summary>
    public enum Effect
    {
        Lottery = 1,
        FishingRarityChanceRareSmall = 2,
        FishingRarityChanceRareMedium = 3,
        FishingRarityChanceRareLarge = 4,
        FishingRarityChanceEpicSmall = 5,
        FishingRarityChanceEpicMedium = 6,
        FishingRarityChanceEpicLarge = 7,
        FishingRarityChanceMythicalSmall = 8,
        FishingRarityChanceMythicalMedium = 9,
        FishingRarityChanceMythicalLarge = 10,
        GatheringDoubleChanceSmall = 11,
        GatheringDoubleChanceMedium = 12,
        GatheringDoubleChanceLarge = 13,
        CraftingDoubleChanceSmall = 14,
        CraftingDoubleChanceMedium = 15,
        CraftingDoubleChanceLarge = 16,
        CookingDoubleChanceSmall = 17,
        CookingDoubleChanceMedium = 18,
        CookingDoubleChanceLarge = 19,
        TradingSellingBoostSmall = 20,
        TradingSellingBoostMedium = 21,
        TradingSellingBoostLarge = 22,
        MovementBoost5 = 23,
        MovementBoost10 = 24,
        MovementBoost20 = 25,
        AlcoholDoubleChanceSmall = 26,
        AlcoholDoubleChanceMedium = 27,
        AlcoholDoubleChanceLarge = 28,
        DailyIenIncome = 29,
        DrinkDoubleChanceSmall = 30,
        DrinkDoubleChanceMedium = 31,
        DrinkDoubleChanceLarge = 32
    }

    public static class CardEffectHelper
    {
        /// <summary>
        /// Возвращает локализированное название эффекта.
        /// </summary>
        /// <param name="effect">Эффект.</param>
        /// <returns>Локализированное название эффекта.</returns>
        public static string Localize(this Effect effect) => effect switch
        {
            Effect.Lottery => "Участник лотереи",
            Effect.FishingRarityChanceRareSmall => "Малое увеличение шанса ловли редкой рыбы",
            Effect.FishingRarityChanceRareMedium => "Среднее увеличение шанса ловли редкой рыбы",
            Effect.FishingRarityChanceRareLarge => "Большое увеличение шанса ловли редкой рыбы",
            Effect.FishingRarityChanceEpicSmall => "Малое увеличение шанса ловли эпической рыбы",
            Effect.FishingRarityChanceEpicMedium => "Среднее увеличение шанса ловли эпической рыбы",
            Effect.FishingRarityChanceEpicLarge => "Большое увеличение шанса ловли эпической рыбы",
            Effect.FishingRarityChanceMythicalSmall => "Малое увеличение шанса ловли мифической рыбы",
            Effect.FishingRarityChanceMythicalMedium => "Среднее увеличение шанса ловли мифической рыбы",
            Effect.FishingRarityChanceMythicalLarge => "Большое увеличение шанса ловли мифической рыбы",
            Effect.GatheringDoubleChanceSmall => "Малое увеличение шанса на сбор дополнительного ресурса",
            Effect.GatheringDoubleChanceMedium => "Среднее увеличение шанса на сбор дополнительного ресурса",
            Effect.GatheringDoubleChanceLarge => "Большое увеличение шанса на сбор дополнительного ресурса",
            Effect.CraftingDoubleChanceSmall => "Малое увеличение шанса на изготовление дополнительного ресурса",
            Effect.CraftingDoubleChanceMedium => "Среднее увеличение шанса на изготовление дополнительного ресурса",
            Effect.CraftingDoubleChanceLarge => "Большое увеличение шанса на изготовление дополнительного ресурса",
            Effect.CookingDoubleChanceSmall => "Малое увеличение шанса на изготовление дополнительного блюда",
            Effect.CookingDoubleChanceMedium => "Среднее увеличение шанса на изготовление дополнительного блюда",
            Effect.CookingDoubleChanceLarge => "Большое увеличение шанса на изготовление дополнительного блюда",
            Effect.TradingSellingBoostSmall => "Малая прибавка дополнительных иен при продаже",
            Effect.TradingSellingBoostMedium => "Средняя прибавка дополнительных иен при продаже",
            Effect.TradingSellingBoostLarge => "Большая прибавка дополнительных иен при продаже",
            Effect.MovementBoost5 => "Увеличение скорости перемещения на 5%",
            Effect.MovementBoost10 => "Увеличение скорости перемещения на 10%",
            Effect.MovementBoost20 => "Увеличение скорости перемещения на 20%",
            Effect.AlcoholDoubleChanceSmall => "Малое увеличение шанса на изготовление дополнительного алкоголя",
            Effect.AlcoholDoubleChanceMedium => "Среднее увеличение шанса на изготовление дополнительного алкоголя",
            Effect.AlcoholDoubleChanceLarge => "Большое увеличение шанса на изготовление дополнительного алкоголя",
            Effect.DailyIenIncome => "Приносит 80 иен ежедневно",
            Effect.DrinkDoubleChanceSmall => "Малое увеличение шанса на изготовление дополнительного напитка",
            Effect.DrinkDoubleChanceMedium => "Среднее увеличение шанса на изготовление дополнительного напитка",
            Effect.DrinkDoubleChanceLarge => "Большое увеличение шанса на изготовление дополнительного напитка",
            _ => throw new ArgumentOutOfRangeException(nameof(effect), effect, null)
        };

        /// <summary>
        /// Возвращает категорию к которой относится эффект.
        /// </summary>
        /// <param name="effect">Эффект.</param>
        /// <returns>Категория эффекта.</returns>
        public static EffectCategory Category(this Effect effect) => effect switch
        {
            Effect.Lottery => EffectCategory.Lottery,
            Effect.FishingRarityChanceRareSmall => EffectCategory.FishingRarityChance,
            Effect.FishingRarityChanceRareMedium => EffectCategory.FishingRarityChance,
            Effect.FishingRarityChanceRareLarge => EffectCategory.FishingRarityChance,
            Effect.FishingRarityChanceEpicSmall => EffectCategory.FishingRarityChance,
            Effect.FishingRarityChanceEpicMedium => EffectCategory.FishingRarityChance,
            Effect.FishingRarityChanceEpicLarge => EffectCategory.FishingRarityChance,
            Effect.FishingRarityChanceMythicalSmall => EffectCategory.FishingRarityChance,
            Effect.FishingRarityChanceMythicalMedium => EffectCategory.FishingRarityChance,
            Effect.FishingRarityChanceMythicalLarge => EffectCategory.FishingRarityChance,
            Effect.GatheringDoubleChanceSmall => EffectCategory.GatheringDoubleChance,
            Effect.GatheringDoubleChanceMedium => EffectCategory.GatheringDoubleChance,
            Effect.GatheringDoubleChanceLarge => EffectCategory.GatheringDoubleChance,
            Effect.CraftingDoubleChanceSmall => EffectCategory.CraftingDoubleChance,
            Effect.CraftingDoubleChanceMedium => EffectCategory.CraftingDoubleChance,
            Effect.CraftingDoubleChanceLarge => EffectCategory.CraftingDoubleChance,
            Effect.CookingDoubleChanceSmall => EffectCategory.CookingDoubleChance,
            Effect.CookingDoubleChanceMedium => EffectCategory.CookingDoubleChance,
            Effect.CookingDoubleChanceLarge => EffectCategory.CookingDoubleChance,
            Effect.TradingSellingBoostSmall => EffectCategory.TradingSellingBoost,
            Effect.TradingSellingBoostMedium => EffectCategory.TradingSellingBoost,
            Effect.TradingSellingBoostLarge => EffectCategory.TradingSellingBoost,
            Effect.MovementBoost5 => EffectCategory.MovementBoost,
            Effect.MovementBoost10 => EffectCategory.MovementBoost,
            Effect.MovementBoost20 => EffectCategory.MovementBoost,
            Effect.AlcoholDoubleChanceSmall => EffectCategory.AlcoholDoubleChance,
            Effect.AlcoholDoubleChanceMedium => EffectCategory.AlcoholDoubleChance,
            Effect.AlcoholDoubleChanceLarge => EffectCategory.AlcoholDoubleChance,
            Effect.DailyIenIncome => EffectCategory.Special,
            Effect.DrinkDoubleChanceSmall => EffectCategory.DrinkDoubleChance,
            Effect.DrinkDoubleChanceMedium => EffectCategory.DrinkDoubleChance,
            Effect.DrinkDoubleChanceLarge => EffectCategory.DrinkDoubleChance,
            _ => throw new ArgumentOutOfRangeException(nameof(effect), effect, null)
        };
    }
}
