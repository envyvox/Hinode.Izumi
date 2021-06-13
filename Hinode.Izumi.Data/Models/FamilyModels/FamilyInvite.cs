using Hinode.Izumi.Data.Models.UserModels;

namespace Hinode.Izumi.Data.Models.FamilyModels
{
    public class FamilyInvite : EntityBase
    {
        public long FamilyId { get; set; }
        public long UserId { get; set; }
        public virtual Family Family { get; set; }
        public virtual User User { get; set; }
    }
}
