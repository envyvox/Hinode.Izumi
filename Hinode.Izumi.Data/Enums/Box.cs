using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Коробка с подарком внутри.
    /// </summary>
    public enum Box
    {
        Capital = 1,
        Garden = 2,
        Seaport = 3,
        Castle = 4,
        Village = 5
    }

    public static class BoxHelper
    {
        /// <summary>
        /// Возвращает локализированное название коробки.
        /// </summary>
        /// <param name="box">Коробка.</param>
        /// <returns>Локализированное название коробки.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Localize(this Box box) => box switch
        {
            Box.Capital => "награда столицы",
            Box.Garden => "награда сада",
            Box.Seaport => "награда порта",
            Box.Castle => "награда замка",
            Box.Village => "награда деревни",
            _ => throw new ArgumentOutOfRangeException(nameof(box), box, null)
        };

        /// <summary>
        /// Возвращает название иконки коробки.
        /// </summary>
        /// <param name="box">Коробка.</param>
        /// <returns>Название иконки коробки.</returns>
        public static string Emote(this Box box) => "Box" + box;
    }
}
