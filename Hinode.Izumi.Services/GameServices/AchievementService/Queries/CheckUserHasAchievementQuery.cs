using MediatR;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Queries
{
    public record CheckUserHasAchievementQuery(long UserId, long AchievementId) : IRequest<bool>;
}
