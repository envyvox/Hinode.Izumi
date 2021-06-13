namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserDrink : EntityBase
    {
        public long UserId { get; set; }
        public long DrinkId { get; set; }
        public long Amount { get; set; }
        public virtual User User { get; set; }
        public virtual Drink Drink { get; set; }
    }
}
