using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetTradingSpecialDiscountQuery(long UserTradingMastery) : IRequest<long>;
}
