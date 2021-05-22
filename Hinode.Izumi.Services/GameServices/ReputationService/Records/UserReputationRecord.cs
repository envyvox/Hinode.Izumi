using Hinode.Izumi.Data.Enums.ReputationEnums;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Records
{
    public record UserReputationRecord(long UserId, Reputation Reputation, long Amount)
    {
        public UserReputationRecord() : this(default, default, default)
        {
        }
    }
}
