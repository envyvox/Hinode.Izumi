using System;

namespace Hinode.Izumi.Data.Enums.RarityEnums
{
    /// <summary>
    /// Редкость рыбы.
    /// </summary>
    public enum FishRarity
    {
        Common = 1,
        Rare = 2,
        Epic = 3,
        Mythical = 4,
        Legendary = 5,
        Divine = 6
    }

    public static class FishRarityHelper
    {
        /// <summary>
        /// Возвращает локализированное название редкости рыбы.
        /// </summary>
        /// <param name="fishRarity">Редкость рыбы.</param>
        /// <param name="declension">Склонение (какую рыбу)?</param>
        /// <returns>Локализированное название редкости рыбы.</returns>
        public static string Localize(this FishRarity fishRarity, bool declension = false) => fishRarity switch
        {
            FishRarity.Common => declension ? "обычную" : "Обычная",
            FishRarity.Rare => declension ? "редкую" : "Редкая",
            FishRarity.Epic => declension ? "эпическую" : "Эпическая",
            FishRarity.Mythical => declension ? "мифическую" : "Мифическая",
            FishRarity.Legendary => declension ? "легендарную" : "Легендарная",
            FishRarity.Divine => declension ? "божественную" : "Божественная",
            _ => throw new ArgumentOutOfRangeException(nameof(fishRarity), fishRarity, null)
        };
    }
}
