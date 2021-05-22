using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record RemoveCraftingIngredientsCommand(long UserId, long CraftingId, long Amount = 1) : IRequest;
}
