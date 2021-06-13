using System;

namespace Hinode.Izumi.Data.Enums
{
    public enum Gender : byte
    {
        None = 0, // пол по-умолчанию
        Male = 1,
        Female = 2
    }

    public static class GenderHelper
    {
        public static string Localize(this Gender gender) => gender switch
        {
            Gender.None => "Не выбран",
            Gender.Male => "Мужской",
            Gender.Female => "Женский",
            _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
        };

        public static string Emote(this Gender gender) => "Gender" + gender;
    }
}
