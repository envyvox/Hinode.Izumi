using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record RemoveFoodIngredientsCommand(long UserId, long FoodId, long Amount = 1) : IRequest;
}
