using System;

namespace Hinode.Izumi.Data.Enums
{
    public enum CollectionCategory : byte
    {
        Gathering = 1,
        Crafting = 2,
        Alcohol = 3,
        Drink = 4,
        Crop = 5,
        Fish = 6,
        Food = 7,
        Event = 8
    }

    public static class CollectionCategoryHelper
    {
        public static string Localize(this CollectionCategory category) => category switch
        {
            CollectionCategory.Gathering => "Собительские предметы",
            CollectionCategory.Crafting => "Изготавливаемые предметы",
            CollectionCategory.Alcohol => "Алкоголь",
            CollectionCategory.Drink => "Напитки",
            CollectionCategory.Crop => "Урожай",
            CollectionCategory.Fish => "Рыба",
            CollectionCategory.Food => "Блюда",
            CollectionCategory.Event => "События",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
