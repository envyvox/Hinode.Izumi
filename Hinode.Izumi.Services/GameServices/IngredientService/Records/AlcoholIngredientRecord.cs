using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Records
{
    public record AlcoholIngredientRecord(
        long AlcoholId,
        IngredientCategory Category,
        long IngredientId,
        long Amount)
    {
        public AlcoholIngredientRecord() : this(default, default, default, default)
        {
        }
    }
}
