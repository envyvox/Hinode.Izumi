using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetTradingMarketTaxPercentQuery(long UserTradingMastery) : IRequest<long>;
}
