using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetTradingTransitDiscountQuery(long UserTradingMastery) : IRequest<long>;
}
