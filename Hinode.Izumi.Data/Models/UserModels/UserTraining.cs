using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserTraining : EntityBase
    {
        public long UserId { get; set; }
        public TutorialStep Step { get; set; }
        public virtual User User { get; set; }
    }
}
