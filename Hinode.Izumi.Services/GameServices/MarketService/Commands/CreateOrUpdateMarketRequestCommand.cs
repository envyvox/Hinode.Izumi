using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Commands
{
    public record CreateOrUpdateMarketRequestCommand(
            long UserId,
            MarketCategory Category,
            long ItemId,
            long Amount,
            long Price,
            bool Selling)
        : IRequest;
}
