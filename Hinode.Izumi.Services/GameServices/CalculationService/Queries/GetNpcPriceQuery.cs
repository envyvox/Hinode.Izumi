using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetNpcPriceQuery(MarketCategory Category, long CostPrice) : IRequest<long>;

    public class GetNpcPriceHandler : IRequestHandler<GetNpcPriceQuery, long>
    {
        private readonly IMediator _mediator;

        public GetNpcPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetNpcPriceQuery request, CancellationToken cancellationToken)
        {
            var (category, costPrice) = request;
            var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice), cancellationToken);
            var markup = await _mediator.Send(new GetPropertyValueQuery(category switch
            {
                MarketCategory.Crafting => Property.CraftingMarkup,
                MarketCategory.Alcohol => Property.CraftingMarkup,
                MarketCategory.Drink => Property.CraftingMarkup,
                MarketCategory.Food => Property.CraftingMarkup,
                _ => throw new ArgumentOutOfRangeException()
            }), cancellationToken);

            return (long) (costPrice + craftingPrice + (costPrice + craftingPrice) / 100.0 * markup);
        }
    }
}
