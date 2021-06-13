namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserProject : EntityBase
    {
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public virtual User User { get; set; }
        public virtual Project Project { get; set; }
    }
}
