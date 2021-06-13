using Hinode.Izumi.Data.Enums.RarityEnums;

namespace Hinode.Izumi.Data.Models
{
    public class Banner : EntityBase
    {
        public BannerRarity Rarity { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public long Price { get; set; }
    }
}
