using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FoodService.Queries
{
    public record GetFoodCostPriceQuery(long Id) : IRequest<long>;

    public class GetFoodCostPriceHandler : IRequestHandler<GetFoodCostPriceQuery, long>
    {
        private readonly IMediator _mediator;

        public GetFoodCostPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetFoodCostPriceQuery request, CancellationToken cancellationToken)
        {
            var ingredients = await _mediator.Send(new GetFoodIngredientsQuery(request.Id), cancellationToken);
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
