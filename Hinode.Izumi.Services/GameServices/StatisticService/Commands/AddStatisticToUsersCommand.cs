using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.StatisticService.Commands
{
    public record AddStatisticToUsersCommand(long[] UsersId, Statistic Statistic, long Amount = 1) : IRequest;
}
