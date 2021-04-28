using System;
using System.Collections.Generic;
using System.Linq;

namespace Hinode.Izumi.Data.Enums.EffectEnums
{
    /// <summary>
    /// Категория эффекта.
    /// </summary>
    public enum EffectCategory
    {
        Lottery = 1,
        FishingRarityChance = 2,
        GatheringDoubleChance = 3,
        CraftingDoubleChance = 4,
        CookingDoubleChance = 5,
        TradingSellingBoost = 6,
        MovementBoost = 7,
        AlcoholDoubleChance = 8,
        Special = 9,
        DrinkDoubleChance = 10
    }

    public static class EffectCategoryHelper
    {
        /// <summary>
        /// Возвращает локализированное название категории эффекта.
        /// </summary>
        /// <param name="category">Категория эффекта.</param>
        /// <returns>Локализированное название категории эффекта.</returns>
        public static string Localize(this EffectCategory category) => category switch
        {
            EffectCategory.Lottery => "Лотерея",
            EffectCategory.FishingRarityChance => "Увеличение шанса ловли определенной редкости рыбы",
            EffectCategory.GatheringDoubleChance => "Увеличение шанса на сбор дополнительного ресурса",
            EffectCategory.CraftingDoubleChance => "Увеличение шанса на изготовление дополнительного ресурса",
            EffectCategory.CookingDoubleChance => "Увеличение шанса на изготовление дополнительного блюда",
            EffectCategory.TradingSellingBoost => "Дополнительные иены при продаже",
            EffectCategory.MovementBoost => "Ускорение перемещения",
            EffectCategory.AlcoholDoubleChance => "Увеличение шанса на изготовление дополнительного алкоголя",
            EffectCategory.Special => "Особые",
            EffectCategory.DrinkDoubleChance => "Увеличение шанса на изготовление дополнительного напитка",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };

        /// <summary>
        /// Возвращает массив эффектов которые находятся в этой категории.
        /// </summary>
        /// <param name="category">Категория.</param>
        /// <returns>Массив эффектов.</returns>
        public static IEnumerable<Effect> Effects(this EffectCategory category) =>
            Enum.GetValues(typeof(Effect))
                .Cast<Effect>()
                .Where(x => x.Category() == category);
    }
}
