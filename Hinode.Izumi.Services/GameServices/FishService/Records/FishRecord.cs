using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.RarityEnums;

namespace Hinode.Izumi.Services.GameServices.FishService.Records
{
    public record FishRecord(
        long Id,
        string Name,
        FishRarity Rarity,
        Season[] Seasons,
        Weather Weather,
        TimesDay TimesDay,
        long Price)
    {
        public FishRecord() : this(default, default, default, default, default, default, default)
        {
        }
    }
}
