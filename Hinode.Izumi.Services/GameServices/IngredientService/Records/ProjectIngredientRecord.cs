using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Records
{
    public record ProjectIngredientRecord(
        long ProjectId,
        IngredientCategory Category,
        long IngredientId,
        long Amount)
    {
        public ProjectIngredientRecord() : this(default, default, default, default)
        {
        }
    }
}
