using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Handlers
{
    public class GetProfitHandler : IRequestHandler<GetProfitQuery, long>
    {
        private readonly IMediator _mediator;

        public GetProfitHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetProfitQuery request, CancellationToken cancellationToken)
        {
            var (costPrice, craftingPrice, npcPrice) = request;
            var marketTaxPercent = await _mediator.Send(new GetTradingMarketTaxPercentQuery(0), cancellationToken);

            return (long) (npcPrice - npcPrice / 100.0 * marketTaxPercent - (costPrice + craftingPrice));
        }
    }
}
