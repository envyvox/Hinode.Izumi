using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record RemoveProjectIngredientsCommand(long UserId, long ProjectId, long Amount = 1) : IRequest;
}
