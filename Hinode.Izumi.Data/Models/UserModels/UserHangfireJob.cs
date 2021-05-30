using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserHangfireJob : EntityBase
    {
        public long UserId { get; set; }
        public HangfireAction Action { get; set; }
        public string JobId { get; set; }
        public virtual User User { get; set; }
    }
}
