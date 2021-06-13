using System;

namespace Hinode.Izumi.Data.Enums
{
    public enum Npc : byte
    {
        Toredo = 1,
        Jodi = 2,
        Toku = 3,
        Ioshiro = 4,
        Ivao = 5,
        Kio = 6,
        Nari = 7
    }

    public static class NpcHelper
    {
        public static string Name(this Npc npc) => npc switch
        {
            Npc.Toredo => "Торедо, успешный торговец",
            Npc.Jodi => "Джоди, владелица казино",
            Npc.Toku => "Току, правитель Эдо",
            Npc.Ioshiro => "Иоширо, “бесстрашный“ шахтер",
            Npc.Ivao => "Ивао, мореплаватель",
            Npc.Kio => "Кио,  добродушный фермер",
            Npc.Nari => "Нари, защитница сада",
            _ => throw new ArgumentOutOfRangeException(nameof(npc), npc, null)
        };

        public static Image Image(this Npc npc) => npc switch
        {
            Npc.Toredo => Enums.Image.NpcCapitalTodedo,
            Npc.Jodi => Enums.Image.NpcCapitalJodi,
            Npc.Toku => Enums.Image.NpcCapitalToku,
            Npc.Ioshiro => Enums.Image.NpcCastleIoshiro,
            Npc.Ivao => Enums.Image.NpcSeaportIvao,
            Npc.Kio => Enums.Image.NpcVillageKio,
            Npc.Nari => Enums.Image.NpcGardenNari,
            _ => throw new ArgumentOutOfRangeException(nameof(npc), npc, null)
        };
    }
}
