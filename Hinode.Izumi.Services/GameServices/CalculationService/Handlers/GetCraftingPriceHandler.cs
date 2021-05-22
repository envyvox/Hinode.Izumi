using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Handlers
{
    public class GetCraftingPriceHandler : IRequestHandler<GetCraftingPriceQuery, long>
    {
        private readonly IMediator _mediator;

        public GetCraftingPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetCraftingPriceQuery request, CancellationToken cancellationToken)
        {
            var (costPrice, amount) = request;
            var craftingPercent = await _mediator.Send(
                new GetPropertyValueQuery(Property.CraftingPricePercent), cancellationToken);

            var craftingPrice = (long) (costPrice / 100.0 * craftingPercent < 1
                    ? 1
                    : costPrice / 100.0 * craftingPercent
                );

            return amount > 0
                ? craftingPrice * amount
                : craftingPrice;
        }
    }
}
