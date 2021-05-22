using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record CheckProjectIngredientsCommand(long UserId, long ProjectId, long Amount = 1) : IRequest;
}
