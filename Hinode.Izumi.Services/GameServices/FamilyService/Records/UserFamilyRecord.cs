using Hinode.Izumi.Data.Enums.FamilyEnums;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Records
{
    public record UserFamilyRecord(long UserId, long FamilyId, UserInFamilyStatus Status)
    {
        public UserFamilyRecord() : this(default, default, default)
        {
        }
    }
}
