using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CraftingService.Queries
{
    public record GetCraftingCostPriceQuery(long Id) : IRequest<long>;

    public class GetCraftingCostPriceHandler : IRequestHandler<GetCraftingCostPriceQuery, long>
    {
        private readonly IMediator _mediator;

        public GetCraftingCostPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetCraftingCostPriceQuery request, CancellationToken cancellationToken)
        {
            var ingredients = await _mediator.Send(new GetCraftingIngredientsQuery(request.Id), cancellationToken);
            long costPrice = 0;

            foreach (var ingredient in ingredients)
            {
                var ingredientCostPrice = await _mediator.Send(
                    new GetIngredientCostPriceQuery(ingredient.Category, ingredient.IngredientId), cancellationToken);
                costPrice += ingredientCostPrice * ingredient.Amount;
            }

            return costPrice;
        }
    }
}
