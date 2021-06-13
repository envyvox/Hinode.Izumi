using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class FoodIngredient : EntityBase
    {
        public long FoodId { get; set; }
        public IngredientCategory Category { get; set; }
        public long IngredientId { get; set; }
        public long Amount { get; set; }
        public virtual Food Food { get; set; }
    }
}
