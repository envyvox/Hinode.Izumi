namespace Hinode.Izumi.Data.Models
{
    public class CraftingProperty : EntityBase
    {
        public long CraftingId { get; set; }
        public Enums.PropertyEnums.CraftingProperty Property { get; set; }
        public long Mastery0 { get; set; }
        public long Mastery50 { get; set; }
        public long Mastery100 { get; set; }
        public long Mastery150 { get; set; }
        public long Mastery200 { get; set; }
        public long Mastery250 { get; set; }
        public virtual Crafting Crafting { get; set; }
    }
}
