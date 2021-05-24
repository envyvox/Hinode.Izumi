using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Records
{
    public record DrinkIngredientRecord(
        long DrinkId,
        IngredientCategory Category,
        long IngredientId,
        long Amount)
    {
        public DrinkIngredientRecord() : this(default, default, default, default)
        {
        }
    }
}
