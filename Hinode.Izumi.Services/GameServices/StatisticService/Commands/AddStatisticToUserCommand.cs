using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.StatisticService.Commands
{
    public record AddStatisticToUserCommand(long UserId, Statistic Statistic, long Amount = 1) : IRequest;
}
