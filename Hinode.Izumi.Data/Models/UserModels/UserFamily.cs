using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Models.FamilyModels;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserFamily : EntityBase
    {
        public long UserId { get; set; }
        public long FamilyId { get; set; }
        public UserInFamilyStatus Status { get; set; }
        public virtual User User { get; set; }
        public virtual Family Family { get; set; }
    }
}
