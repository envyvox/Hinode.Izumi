using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Handlers
{
    public class CheckAchievementsInUserHandler : IRequestHandler<CheckAchievementsInUserCommand>
    {
        private readonly IMediator _mediator;

        public CheckAchievementsInUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckAchievementsInUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, achievements) = request;

            foreach (var achievement in achievements)
            {
                await _mediator.Send(new CheckAchievementInUserCommand(userId, achievement), cancellationToken);
            }

            return new Unit();
        }
    }
}
