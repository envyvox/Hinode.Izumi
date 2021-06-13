namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserRecipe : EntityBase
    {
        public long UserId { get; set; }
        public long FoodId { get; set; }
        public virtual User User { get; set; }
        public virtual Food Food { get; set; }
    }
}
