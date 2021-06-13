using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class CraftingIngredient : EntityBase
    {
        public long CraftingId { get; set; }
        public IngredientCategory Category { get; set; }
        public long IngredientId { get; set; }
        public long Amount { get; set; }
        public virtual Crafting Crafting { get; set; }
    }
}
