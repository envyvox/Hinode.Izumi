using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record CheckFoodIngredientsCommand(long UserId, long FoodId, long Amount = 1) : IRequest;

    public class CheckFoodIngredientsHandler : IRequestHandler<CheckFoodIngredientsCommand>
    {
        private readonly IMediator _mediator;

        public CheckFoodIngredientsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckFoodIngredientsCommand request, CancellationToken cancellationToken)
        {
            var (userId, foodId, amount) = request;
            var ingredients = await _mediator.Send(new GetFoodIngredientsQuery(foodId), cancellationToken);

            foreach (var ingredient in ingredients)
                await _mediator.Send(new CheckIngredientAmountCommand(
                        userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount, amount),
                    cancellationToken);

            return new Unit();
        }
    }
}
