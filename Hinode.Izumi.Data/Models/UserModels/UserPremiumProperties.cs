namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserPremiumProperties : EntityBase
    {
        public long UserId { get; set; }
        public string CommandColor { get; set; }
        public virtual User User { get; set; }
    }
}
