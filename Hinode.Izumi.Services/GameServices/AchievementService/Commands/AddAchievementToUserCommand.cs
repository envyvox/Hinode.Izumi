using MediatR;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Commands
{
    public record AddAchievementToUserCommand(long UserId, long AchievementId) : IRequest;
}
