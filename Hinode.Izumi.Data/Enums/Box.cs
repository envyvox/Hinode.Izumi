using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Коробка с подарком внутри.
    /// </summary>
    public enum Box
    {
        CapitalBossReward = 1,
        GardenBossReward = 2,
        SeaportBossReward = 3,
        CastleBossReward = 4,
        VillageBossReward = 5
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
            Box.CapitalBossReward => "награда столицы",
            Box.GardenBossReward => "награда сада",
            Box.SeaportBossReward => "награда порта",
            Box.CastleBossReward => "награда замка",
            Box.VillageBossReward => "награда деревни",
            _ => throw new ArgumentOutOfRangeException(nameof(box), box, null)
        };

        /// <summary>
        /// Возвращает название иконки коробки.
        /// </summary>
        /// <param name="box">Коробка.</param>
        /// <returns>Название иконки коробки.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Emote(this Box box) => box switch
        {
            Box.CapitalBossReward => "",
            Box.GardenBossReward => "",
            Box.SeaportBossReward => "",
            Box.CastleBossReward => "",
            Box.VillageBossReward => "",
            _ => throw new ArgumentOutOfRangeException(nameof(box), box, null)
        };
    }
}
