using Hinode.Izumi.Services.GameServices.AchievementService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Queries
{
    public record GetAchievementByIdQuery(long Id) : IRequest<AchievementRecord>;
}
