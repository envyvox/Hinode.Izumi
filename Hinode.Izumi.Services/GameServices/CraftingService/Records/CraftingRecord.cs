using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.CraftingService.Records
{
    public record CraftingRecord(long Id, string Name, long Time, Location Location)
    {
        public CraftingRecord() : this(default, default, default, default)
        {
        }
    }
}
