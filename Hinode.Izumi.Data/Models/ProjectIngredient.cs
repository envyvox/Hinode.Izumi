using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class ProjectIngredient : EntityBase
    {
        public long ProjectId { get; set; }
        public IngredientCategory Category { get; set; }
        public long IngredientId { get; set; }
        public long Amount { get; set; }
        public virtual Project Project { get; set; }
    }
}
