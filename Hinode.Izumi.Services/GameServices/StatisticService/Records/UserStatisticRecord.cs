using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.StatisticService.Records
{
    public record UserStatisticRecord(long UserId, Statistic Statistic, long Amount)
    {
        public UserStatisticRecord() : this(default, default, default)
        {
        }
    }
}
