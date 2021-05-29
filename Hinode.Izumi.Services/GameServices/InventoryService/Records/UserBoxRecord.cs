using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Records
{
    public record UserBoxRecord(
        long UserId,
        Box Box,
        long Amount)
    {
        public UserBoxRecord() : this(default, default, default)
        {
        }
    }
}
