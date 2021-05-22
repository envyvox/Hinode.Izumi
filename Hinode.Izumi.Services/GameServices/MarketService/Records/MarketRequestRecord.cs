using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.MarketService.Records
{
    public record MarketRequestRecord(
        long Id,
        long UserId,
        MarketCategory Category,
        long ItemId,
        long Price,
        long Amount,
        bool Selling)
    {
        public MarketRequestRecord() : this(default, default, default, default, default, default, default)
        {
        }
    }
}
