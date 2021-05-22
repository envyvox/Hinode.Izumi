using Hinode.Izumi.Data.Enums.AchievementEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Commands
{
    public record CheckAchievementInUsersCommand(long[] UsersId, Achievement Type) : IRequest;
}
