using System;

namespace Hinode.Izumi.Data.Enums
{
    public enum Mastery : byte
    {
        Gathering = 1,
        Fishing = 2,
        Cooking = 3,
        Crafting = 4,
        Trading = 5,
        Alchemy = 6
    }

    public static class MasteryHelper
    {
        public static string Localize(this Mastery mastery) => mastery switch
        {
            Mastery.Gathering => "Сбор",
            Mastery.Fishing => "Рыбалка",
            Mastery.Cooking => "Кулинария",
            Mastery.Crafting => "Изготовление",
            Mastery.Trading => "Торговля",
            Mastery.Alchemy => "Алхимия",
            _ => throw new ArgumentOutOfRangeException(nameof(mastery), mastery, null)
        };

        public static string Description(this Mastery mastery) => mastery switch
        {
            Mastery.Gathering =>
                "При исследованиях вы будете **чаще** и **больше** получать ресурсы.",

            Mastery.Fishing =>
                "Ваши навыки позволят вам не только вылавливать **чаще**, но и более **редкая** рыба станет вам \"по зубам\".",

            Mastery.Cooking =>
                "Вам станут доступными **новые** кулинарные **рецепты**.",

            Mastery.Crafting =>
                "Ваши руки настолько приспособлятся к изготовлению, что создание **дополнительного** предмета станет нормой.",

            Mastery.Trading =>
                "Торговцы будут **лояльнее** относиться к вам, делая различные **скидки**.",

            Mastery.Alchemy =>
                "Ожидайте обновления...",

            _ => throw new ArgumentOutOfRangeException(nameof(mastery), mastery, null)
        };
    }
}
