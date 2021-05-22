using MediatR;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Commands
{
    public record AddAchievementRewardToUserCommand(long UserId, long AchievementId) : IRequest;
}
