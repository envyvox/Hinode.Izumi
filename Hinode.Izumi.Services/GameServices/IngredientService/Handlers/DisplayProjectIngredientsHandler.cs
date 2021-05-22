using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Handlers
{
    public class DisplayProjectIngredientsHandler : IRequestHandler<DisplayProjectIngredientsQuery, string>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public DisplayProjectIngredientsHandler(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<string> Handle(DisplayProjectIngredientsQuery request, CancellationToken cancellationToken)
        {
            var (projectId, amount) = request;
            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            var ingredients = await _mediator.Send(new GetProjectIngredientsQuery(projectId), cancellationToken);

            var ingredientsString = string.Empty;
            foreach (var ingredient in ingredients)
            {
                var ingredientName = await _mediator.Send(
                    new GetIngredientNameQuery(ingredient.Category, ingredient.IngredientId), cancellationToken);

                ingredientsString +=
                    $"{emotes.GetEmoteOrBlank(ingredientName)} {ingredient.Amount * amount} {_local.Localize(ingredientName, ingredient.Amount * amount)}, ";
            }

            return ingredientsString.Remove(ingredientsString.Length - 2);
        }
    }
}
