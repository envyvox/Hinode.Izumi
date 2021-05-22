using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.DrinkService.Queries;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.DrinkService.Handlers
{
    public class GetDrinkCostPriceHandler : IRequestHandler<GetDrinkCostPriceQuery, long>
    {
        private readonly IMediator _mediator;

        public GetDrinkCostPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetDrinkCostPriceQuery request, CancellationToken cancellationToken)
        {
            var ingredients = await _mediator.Send(new GetDrinkIngredientsQuery(request.Id), cancellationToken);
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
