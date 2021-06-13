using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;

namespace Hinode.Izumi.Data.Models
{
    public class Card : EntityBase
    {
        public CardRarity Rarity { get; set; }
        public Effect Effect { get; set; }
        public string Name { get; set; }
        public string Anime { get; set; }
        public string Url { get; set; }
    }
}
