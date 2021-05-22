using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Queries
{
    public record DisplayCraftingIngredientQuery(long CraftingId, long Amount = 1) : IRequest<string>;
}
