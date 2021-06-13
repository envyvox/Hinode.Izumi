using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class AlcoholIngredient : EntityBase
    {
        public long AlcoholId { get; set; }
        public IngredientCategory Category { get; set; }
        public long IngredientId { get; set; }
        public long Amount { get; set; }
        public virtual Alcohol Alcohol { get; set; }
    }
}
