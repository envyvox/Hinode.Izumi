using Hinode.Izumi.Data.Enums.RarityEnums;

namespace Hinode.Izumi.Services.GameServices.BannerService.Records
{
    public record BannerRecord(long Id, BannerRarity Rarity, string Name, string Url, long Price)
    {
        public BannerRecord() : this(default, default, default, default, default)
        {
        }
    }
}
