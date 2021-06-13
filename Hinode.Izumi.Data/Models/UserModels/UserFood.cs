namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserFood : EntityBase
    {
        public long UserId { get; set; }
        public long FoodId { get; set; }
        public long Amount { get; set; }
        public virtual User User { get; set; }
        public virtual Food Food { get; set; }
    }
}
