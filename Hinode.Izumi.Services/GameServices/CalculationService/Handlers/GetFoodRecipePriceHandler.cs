using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Handlers
{
    public class GetFoodRecipePriceHandler : IRequestHandler<GetFoodRecipePriceQuery, long>
    {
        private readonly IMediator _mediator;

        public GetFoodRecipePriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetFoodRecipePriceQuery request, CancellationToken cancellationToken)
        {
            var craftingPrice = await _mediator.Send(
                new GetCraftingPriceQuery(request.CostPrice), cancellationToken);
            var npcPrice = await _mediator.Send(
                new GetNpcPriceQuery(MarketCategory.Food, request.CostPrice), cancellationToken);
            var profit = await _mediator.Send(
                new GetProfitQuery(request.CostPrice, craftingPrice, npcPrice), cancellationToken);
            var paybackSales = await _mediator.Send(
                new GetPropertyValueQuery(Property.RecipePaybackSales), cancellationToken);

            return profit * paybackSales;
        }
    }
}
