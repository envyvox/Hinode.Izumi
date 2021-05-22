using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record CheckDrinkIngredientsCommand(long UserId, long DrinkId, long Amount = 1) : IRequest;
}
