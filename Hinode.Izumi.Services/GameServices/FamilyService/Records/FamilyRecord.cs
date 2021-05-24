using Hinode.Izumi.Data.Enums.FamilyEnums;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Records
{
    public record FamilyRecord(long Id, FamilyStatus Status, string Name, string Description)
    {
        public FamilyRecord() : this(default, default, default, default)
        {
        }
    }
}
