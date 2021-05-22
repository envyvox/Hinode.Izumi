using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Commands
{
    public record UpdateOrDeleteMarketRequestCommand(
            MarketCategory Category,
            long ItemId,
            long MarketAmount,
            long Amount)
        : IRequest;
}
