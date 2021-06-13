namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserCrafting : EntityBase
    {
        public long UserId { get; set; }
        public long CraftingId { get; set; }
        public long Amount { get; set; }
        public virtual User User { get; set; }
        public virtual Crafting Crafting { get; set; }
    }
}
