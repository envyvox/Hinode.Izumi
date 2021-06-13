using System;

namespace Hinode.Izumi.Data.Enums.RarityEnums
{
    public enum CardRarity : byte
    {
        Common = 1,
        Rare = 2,
        Legendary = 3,
        Special = 4
    }

    public static class CardRarityHelper
    {
        public static string Localize(this CardRarity rarity, bool declension = false) => rarity switch
        {
            CardRarity.Common => declension ? "обычную карточку" : "Обычная карточка",
            CardRarity.Rare => declension ? "редкую карточку" : "Редкая карточка",
            CardRarity.Legendary => declension ? "легендарную карточку" : "Легендарная карточка",
            CardRarity.Special => declension ? "особую карточку" : "Особая карточка",
            _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
        };
    }
}
