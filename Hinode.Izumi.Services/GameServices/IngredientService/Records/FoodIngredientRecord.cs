using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Records
{
    public record FoodIngredientRecord(
        long FoodI,
        IngredientCategory Category,
        long IngredientId,
        long Amount)
    {
        public FoodIngredientRecord() : this(default, default, default, default)
        {
        }
    }
}
