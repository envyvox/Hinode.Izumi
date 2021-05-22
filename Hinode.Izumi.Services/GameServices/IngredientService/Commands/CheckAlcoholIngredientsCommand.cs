using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record CheckAlcoholIngredientsCommand(long UserId, long AlcoholId, long Amount = 1) : IRequest;
}
