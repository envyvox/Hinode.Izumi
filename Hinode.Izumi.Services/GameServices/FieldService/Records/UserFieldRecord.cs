using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.FieldService.Records
{
    public record UserFieldRecord(
        long Id,
        long UserId,
        long FieldId,
        FieldState State,
        long SeedId,
        long Progress,
        bool ReGrowth)
    {
        public UserFieldRecord() : this(default, default, default, default, default, default, default)
        {
        }
    }
}
