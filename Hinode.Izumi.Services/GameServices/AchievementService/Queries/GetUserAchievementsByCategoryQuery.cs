using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Services.GameServices.AchievementService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Queries
{
    public record GetUserAchievementsByCategoryQuery(
            long UserId,
            AchievementCategory Category)
        : IRequest<UserAchievementRecord[]>;
}
