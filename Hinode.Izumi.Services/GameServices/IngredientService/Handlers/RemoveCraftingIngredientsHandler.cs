using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.IngredientService.Commands;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Handlers
{
    public class RemoveCraftingIngredientsHandler : IRequestHandler<RemoveCraftingIngredientsCommand>
    {
        private readonly IMediator _mediator;

        public RemoveCraftingIngredientsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveCraftingIngredientsCommand request, CancellationToken cancellationToken)
        {
            var (userId, craftingId, amount) = request;
            var ingredients = await _mediator.Send(new GetCraftingIngredientsQuery(craftingId), cancellationToken);

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
