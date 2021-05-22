using Hinode.Izumi.Data.Enums.AchievementEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Commands
{
    public record CheckAchievementsInUsersCommand(long[] UsersId, Achievement[] Achievements) : IRequest;
}
