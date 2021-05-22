using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.AlcoholService.Handlers
{
    public class GetAlcoholCostPriceHandler : IRequestHandler<GetAlcoholCostPriceQuery, long>
    {
        private readonly IMediator _mediator;

        public GetAlcoholCostPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetAlcoholCostPriceQuery request, CancellationToken cancellationToken)
        {
            var ingredients = await _mediator.Send(new GetAlcoholIngredientsQuery(request.Id), cancellationToken);
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
