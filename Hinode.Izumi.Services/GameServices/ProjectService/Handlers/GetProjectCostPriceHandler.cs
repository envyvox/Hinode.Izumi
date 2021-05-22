using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using Hinode.Izumi.Services.GameServices.ProjectService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ProjectService.Handlers
{
    public class GetProjectCostPriceHandler : IRequestHandler<GetProjectCostPriceQuery, long>
    {
        private readonly IMediator _mediator;

        public GetProjectCostPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetProjectCostPriceQuery request, CancellationToken cancellationToken)
        {
            var ingredients = await _mediator.Send(new GetProjectIngredientsQuery(request.Id), cancellationToken);
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
