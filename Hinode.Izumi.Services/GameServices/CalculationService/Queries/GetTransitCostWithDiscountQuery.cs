using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetTransitCostWithDiscountQuery(long UserTradingMastery, long TransitPrice) : IRequest<long>;
}
