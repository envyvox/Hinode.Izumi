using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetCurrencyAmountAfterMarketTaxQuery(long UserTradingMastery, long Amount) : IRequest<long>;
}
