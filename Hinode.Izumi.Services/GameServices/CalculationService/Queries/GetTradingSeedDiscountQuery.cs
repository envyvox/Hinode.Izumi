using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetTradingSeedDiscountQuery(long UserTradingMastery) : IRequest<long>;
}
