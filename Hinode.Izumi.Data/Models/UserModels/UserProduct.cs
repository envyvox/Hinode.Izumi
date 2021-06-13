namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserProduct : EntityBase
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public long Amount { get; set; }
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
