using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.StatisticService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.StatisticService.Queries
{
    public record GetUserStatisticQuery(long UserId, Statistic Statistic) : IRequest<UserStatisticRecord>;
}
