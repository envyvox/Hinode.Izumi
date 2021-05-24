using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Records
{
    public record CraftingIngredientRecord(
        long CraftingId,
        IngredientCategory Category,
        long IngredientId,
        long Amount)
    {
        public CraftingIngredientRecord() : this(default, default, default, default)
        {
        }
    }
}
