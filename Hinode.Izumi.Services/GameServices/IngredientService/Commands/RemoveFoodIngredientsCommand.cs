using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record RemoveFoodIngredientsCommand(long UserId, long FoodId, long Amount = 1) : IRequest;

    public class RemoveFoodIngredientsHandler : IRequestHandler<RemoveFoodIngredientsCommand>
    {
        private readonly IMediator _mediator;

        public RemoveFoodIngredientsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveFoodIngredientsCommand request, CancellationToken cancellationToken)
        {
            var (userId, foodId, amount) = request;
            var ingredients = await _mediator.Send(new GetFoodIngredientsQuery(foodId), cancellationToken);

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
