using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record CheckFoodIngredientsCommand(long UserId, long FoodId, long Amount = 1) : IRequest;
}
