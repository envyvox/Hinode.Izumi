using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetSeedPriceWithDiscountQuery(long UserTradingMastery, long SeedPrice) : IRequest<long>;

    public class GetSeedPriceWithDiscountHandler : IRequestHandler<GetSeedPriceWithDiscountQuery, long>
    {
        private readonly IMediator _mediator;

        public GetSeedPriceWithDiscountHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetSeedPriceWithDiscountQuery request, CancellationToken cancellationToken)
        {
            var (userTradingMastery, seedPrice) = request;

            if (userTradingMastery < 50) return seedPrice;

            var discount = (await _mediator.Send(
                    new GetMasteryPropertiesQuery(MasteryProperty.TradingMasterySeedDiscount), cancellationToken))
                .MasteryMaxValue(userTradingMastery);
            var priceWithDiscount = seedPrice - seedPrice * discount / 100;

            return priceWithDiscount > 0 ? priceWithDiscount : 1;
        }
    }
}
