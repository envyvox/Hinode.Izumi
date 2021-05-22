using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.MarketService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Queries
{
    public record GetMarketRequestByParamsQuery(
            MarketCategory Category,
            bool Selling,
            string NamePattern = null)
        : IRequest<MarketRequestRecord[]>;
}
