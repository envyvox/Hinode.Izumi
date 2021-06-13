using System;

namespace Hinode.Izumi.Data.Enums
{
    public enum BossDebuff : byte
    {
        None = 0,
        CapitalStop = 1,
        GardenStop = 2,
        SeaportStop = 3,
        CastleStop = 4,
        VillageStop = 5
    }

    public static class BossDebuffHelper
    {
        public static string Localize(this BossDebuff debuff) => debuff switch
        {
            BossDebuff.None =>
                "Сегодня ежедневный босс был **убит**, никаких последствий, отличная работа!",

            BossDebuff.CapitalStop =>
                "Все жители отправились ремонтировать свои дома и приводить улицы в порядок, поэтому все заведения будут закрыты.",

            BossDebuff.GardenStop =>
                "Из-за огромного количества поваленных деревьев работа сада была приостановлена на время восстановления.",

            BossDebuff.SeaportStop =>
                "Прибрежную зону разрушили огромные волны, жители проведут сегодняшний день восстанавливая ее.",

            BossDebuff.CastleStop =>
                "Шахта, приносящая доход практически каждому жителю, оказалась заваленной, поэтому работа замка была приостановлена чтобы расчистить ее.",

            BossDebuff.VillageStop =>
                "Все поля с урожаями пострадали и уж точно не продвинуться в росте за сегодняшний день. Это огромный удар для жителей, сегодня у них траур.",

            _ => throw new ArgumentOutOfRangeException(nameof(debuff), debuff, null)
        };

        public static Location Location(this BossDebuff debuff) => debuff switch
        {
            BossDebuff.CapitalStop => Enums.Location.Capital,
            BossDebuff.GardenStop => Enums.Location.Garden,
            BossDebuff.SeaportStop => Enums.Location.Seaport,
            BossDebuff.CastleStop => Enums.Location.Castle,
            BossDebuff.VillageStop => Enums.Location.Village,
            // отсутствие последствий не может относиться к конкретной локации
            BossDebuff.None => throw new ArgumentOutOfRangeException(nameof(debuff), debuff, null),
            _ => throw new ArgumentOutOfRangeException(nameof(debuff), debuff, null)
        };
    }
}
