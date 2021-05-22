using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record RemoveDrinkIngredientsCommand(long UserId, long DrinkId, long Amount = 1) : IRequest;
}
