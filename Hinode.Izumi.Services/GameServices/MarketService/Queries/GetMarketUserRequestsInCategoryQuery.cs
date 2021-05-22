using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.MarketService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Queries
{
    public record GetMarketUserRequestsInCategoryQuery(
            long UserId,
            MarketCategory Category)
        : IRequest<MarketRequestRecord[]>;
}
