using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.RarityEnums;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Records
{
    public record UserFishRecord(
        long UserId,
        long FishId,
        long Amount,
        string Name,
        FishRarity Rarity,
        Season[] Seasons,
        long Price)
    {
        public UserFishRecord() : this(default, default, default, default, default, default, default)
        {
        }
    }
}
