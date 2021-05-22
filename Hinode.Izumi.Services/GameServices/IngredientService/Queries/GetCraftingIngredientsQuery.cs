using Hinode.Izumi.Services.GameServices.IngredientService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Queries
{
    public record GetCraftingIngredientsQuery(long CraftingId) : IRequest<CraftingIngredientRecord[]>;
}
