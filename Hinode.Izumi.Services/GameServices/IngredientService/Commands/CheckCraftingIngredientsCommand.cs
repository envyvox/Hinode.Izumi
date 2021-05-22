using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record CheckCraftingIngredientsCommand(long UserId, long CraftingId, long Amount = 1) : IRequest;
}
