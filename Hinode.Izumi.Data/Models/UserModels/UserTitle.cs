using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserTitle : EntityBase
    {
        public long UserId { get; set; }
        public Title Title { get; set; }
        public virtual User User { get; set; }
    }
}
