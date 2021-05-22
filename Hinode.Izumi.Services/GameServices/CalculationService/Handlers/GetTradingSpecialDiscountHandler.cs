using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Handlers
{
    public class GetTradingSpecialDiscountHandler : IRequestHandler<GetTradingSpecialDiscountQuery, long>
    {
        private readonly IMediator _mediator;

        public GetTradingSpecialDiscountHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetTradingSpecialDiscountQuery request, CancellationToken cancellationToken)
        {
            return (await _mediator.Send(
                    new GetMasteryPropertiesQuery(MasteryProperty.TradingMasterySpecialOfferDiscount),
                    cancellationToken))
                .MasteryMaxValue(request.UserTradingMastery);
        }
    }
}
