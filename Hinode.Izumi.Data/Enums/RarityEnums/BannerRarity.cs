using System;

namespace Hinode.Izumi.Data.Enums.RarityEnums
{
    public enum BannerRarity : byte
    {
        Common = 1,
        Rare = 2,
        Animated = 3,
        Personal = 4,
        Event = 5
    }

    public static class BannerRarityHelper
    {
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
