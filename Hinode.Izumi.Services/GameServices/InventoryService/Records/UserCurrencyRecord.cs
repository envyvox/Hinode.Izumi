using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Records
{
    public record UserCurrencyRecord(
        long UserId,
        Currency Currency,
        long Amount)
    {
        public UserCurrencyRecord() : this(default, default, default)
        {
        }
    }
}
