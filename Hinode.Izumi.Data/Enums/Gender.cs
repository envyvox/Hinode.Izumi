using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Пол пользователя.
    /// </summary>
    public enum Gender
    {
        None = 0, // пол по-умолчанию
        Male = 1,
        Female = 2
    }

    public static class GenderHelper
    {
        /// <summary>
        /// Возвращает локализированное название пола.
        /// </summary>
        /// <param name="gender">Пол.</param>
        /// <returns>Локализированное название пола.</returns>
        public static string Localize(this Gender gender) => gender switch
        {
            Gender.None => "Не выбран",
            Gender.Male => "Мужской",
            Gender.Female => "Женский",
            _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
        };

        /// <summary>
        /// Возвращает название иконки для отображения пола.
        /// </summary>
        /// <param name="gender">Пол.</param>
        /// <returns>Название иконки для отображения пола.</returns>
        public static string Emote(this Gender gender) => "Gender" + gender;
    }
}
