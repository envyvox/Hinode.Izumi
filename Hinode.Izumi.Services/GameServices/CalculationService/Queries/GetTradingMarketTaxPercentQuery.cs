using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetTradingMarketTaxPercentQuery(long UserTradingMastery) : IRequest<long>;

    public class GetTradingMarketTaxPercentHandler : IRequestHandler<GetTradingMarketTaxPercentQuery, long>
    {
        private readonly IMediator _mediator;

        public GetTradingMarketTaxPercentHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetTradingMarketTaxPercentQuery request, CancellationToken cancellationToken)
        {
            return (await _mediator.Send(
                    new GetMasteryPropertiesQuery(MasteryProperty.TradingMasteryMarketTax), cancellationToken))
                .MasteryMaxValue(request.UserTradingMastery);
        }
    }
}
