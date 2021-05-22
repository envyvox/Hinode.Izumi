using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.IngredientService.Commands;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Handlers
{
    public class RemoveProjectIngredientsHandler : IRequestHandler<RemoveProjectIngredientsCommand>
    {
        private readonly IMediator _mediator;

        public RemoveProjectIngredientsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveProjectIngredientsCommand request, CancellationToken cancellationToken)
        {
            var (userId, projectId, amount) = request;
            var ingredients = await _mediator.Send(new GetProjectIngredientsQuery(projectId), cancellationToken);

            foreach (var ingredient in ingredients)
            {
                await _mediator.Send(new RemoveItemFromUserByIngredientCategoryCommand(
                        userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount * amount),
                    cancellationToken);
            }

            return new Unit();
        }
    }
}
