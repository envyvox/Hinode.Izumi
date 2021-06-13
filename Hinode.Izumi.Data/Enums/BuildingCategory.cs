using System;

namespace Hinode.Izumi.Data.Enums
{
    public enum BuildingCategory : byte
    {
        Personal = 1,
        Family = 2,
        Clan = 3
    }

    public static class BuildingCategoryHelper
    {
        public static string Localize(this BuildingCategory category, bool declension = false) => category switch
        {
            BuildingCategory.Personal => declension ? "Персональный" : "Персональные",
            BuildingCategory.Family => declension ? "Семейный" : "Семейные",
            BuildingCategory.Clan => declension ? "Клановый" : "Клановые",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
