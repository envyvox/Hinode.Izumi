using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.MasteryService.Records
{
    public record UserMasteryRecord(long UserId, Mastery Mastery, double Amount)
    {
        public UserMasteryRecord() : this(default, default, default)
        {
        }
    }
}
