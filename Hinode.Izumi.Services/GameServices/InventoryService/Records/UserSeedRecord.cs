using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Records
{
    public record UserSeedRecord(
        long UserId,
        long SeedId,
        long Amount,
        string Name,
        Season Season)
    {
        public UserSeedRecord() : this(default, default, default, default, default)
        {
        }
    }
}
