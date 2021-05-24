using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetTransitCostWithDiscountQuery(long UserTradingMastery, long TransitPrice) : IRequest<long>;

    public class GetTransitCostWithDiscountHandler : IRequestHandler<GetTransitCostWithDiscountQuery, long>
    {
        private readonly IMediator _mediator;

        public GetTransitCostWithDiscountHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetTransitCostWithDiscountQuery request, CancellationToken cancellationToken)
        {
            var (userTradingMastery, transitPrice) = request;

            if (transitPrice == 0) return 0;

            var discount = await _mediator.Send(
                new GetTradingTransitDiscountQuery(userTradingMastery), cancellationToken);
            var priceWithDiscount = transitPrice - transitPrice * discount / 100;

            return priceWithDiscount > 0 ? priceWithDiscount : 1;
        }
    }
}
