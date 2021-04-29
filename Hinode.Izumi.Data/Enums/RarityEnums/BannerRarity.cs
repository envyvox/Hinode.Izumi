using System;

namespace Hinode.Izumi.Data.Enums.RarityEnums
{
    /// <summary>
    /// Редкость баннера.
    /// </summary>
    public enum BannerRarity
    {
        Common = 1,
        Rare = 2,
        Animated = 3,
        Personal = 4,
        Event = 5
    }

    public static class BannerRarityHelper
    {
        /// <summary>
        /// Возвращает локализированное название редкости баннера.
        /// </summary>
        /// <param name="bannerRarity">Редкость баннера.</param>
        /// <returns>Локализированное название редкости баннера</returns>
        public static string Localize(this BannerRarity bannerRarity) => bannerRarity switch
        {
            BannerRarity.Common => "Обычный баннер",
            BannerRarity.Rare => "Редкий баннер",
            BannerRarity.Animated => "Анимированный баннер",
            BannerRarity.Personal => "Персональный баннер",
            BannerRarity.Event => "Баннер события",
            _ => throw new ArgumentOutOfRangeException(nameof(bannerRarity), bannerRarity, null)
        };
    }
}
