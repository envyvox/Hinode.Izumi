using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.TutorialService.Queries
{
    public record GetUserTutorialStepQuery(long UserId) : IRequest<TutorialStep>;
}
