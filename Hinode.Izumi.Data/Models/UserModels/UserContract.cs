namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserContract : EntityBase
    {
        public long UserId { get; set; }
        public long ContractId { get; set; }
        public virtual User User { get; set; }
        public virtual Contract Contract { get; set; }
    }
}
