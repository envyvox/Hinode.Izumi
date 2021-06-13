namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserAlcohol : EntityBase
    {
        public long UserId { get; set; }
        public long AlcoholId { get; set; }
        public long Amount { get; set; }
        public virtual User User { get; set; }
        public virtual Alcohol Alcohol { get; set; }
    }
}
