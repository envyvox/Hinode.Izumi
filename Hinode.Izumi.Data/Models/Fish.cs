using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.RarityEnums;

namespace Hinode.Izumi.Data.Models
{
    public class Fish : EntityBase
    {
        public string Name { get; set; }
        public FishRarity Rarity { get; set; }
        public Season[] Seasons { get; set; }
        public Weather Weather { get; set; }
        public TimesDay TimesDay { get; set; }
        public long Price { get; set; }
    }
}
