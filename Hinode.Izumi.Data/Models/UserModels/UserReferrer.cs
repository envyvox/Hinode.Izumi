namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserReferrer : EntityBase
    {
        public long UserId { get; set; }
        public long ReferrerId { get; set; }
        public virtual User User { get; set; }
        public virtual User Referrer { get; set; }
    }
}
