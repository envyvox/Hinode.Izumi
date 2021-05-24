using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FoodService.Queries
{
    public record GetFoodSeasonsQuery(long Id) : IRequest<List<Season>>;

    public class GetFoodSeasonsHandler : IRequestHandler<GetFoodSeasonsQuery, List<Season>>
    {
        private readonly IMediator _mediator;

        public GetFoodSeasonsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<Season>> Handle(GetFoodSeasonsQuery request, CancellationToken cancellationToken)
        {
            var ingredients = await _mediator.Send(new GetFoodIngredientsQuery(request.Id), cancellationToken);
            var seasons = new List<Season>();

            foreach (var ingredient in ingredients)
            {
                var ingredientSeasons = await _mediator.Send(
                    new GetIngredientSeasonsQuery(ingredient.Category, ingredient.IngredientId), cancellationToken);

                foreach (var season in ingredientSeasons)
                {
                    if (!seasons.Contains(season)) seasons.Add(season);
                }
            }

            seasons.Sort();
            return seasons;
        }
    }
}
