using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Handlers
{
    public class GetCurrencyAmountAfterMarketTaxHandler : IRequestHandler<GetCurrencyAmountAfterMarketTaxQuery, long>
    {
        private readonly IMediator _mediator;

        public GetCurrencyAmountAfterMarketTaxHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetCurrencyAmountAfterMarketTaxQuery request,
            CancellationToken cancellationToken)
        {
            var (userTradingMastery, amount) = request;
            var marketTaxPercent = await _mediator.Send(
                new GetTradingMarketTaxPercentQuery(userTradingMastery), cancellationToken);

            return amount - amount * marketTaxPercent / 100;
        }
    }
}
