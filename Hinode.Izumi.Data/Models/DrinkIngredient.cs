using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class DrinkIngredient : EntityBase
    {
        public long DrinkId { get; set; }
        public IngredientCategory Category { get; set; }
        public long IngredientId { get; set; }
        public long Amount { get; set; }
        public virtual Drink Drink { get; set; }
    }
}
