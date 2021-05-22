using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.MarketService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Queries
{
    public record GetMarketUserRequestQuery(
            long UserId,
            MarketCategory Category,
            long ItemId)
        : IRequest<MarketRequestRecord>;
}
