using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetSeedPriceWithDiscountQuery(long UserTradingMastery, long SeedPrice) : IRequest<long>;
}
