using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record RemoveAlcoholIngredientsCommand(long UserId, long AlcoholId, long Amount = 1) : IRequest;
}
