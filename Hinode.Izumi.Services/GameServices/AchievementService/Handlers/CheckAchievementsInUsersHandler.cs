using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Handlers
{
    public class CheckAchievementsInUsersHandler : IRequestHandler<CheckAchievementsInUsersCommand>
    {
        private readonly IMediator _mediator;

        public CheckAchievementsInUsersHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckAchievementsInUsersCommand request, CancellationToken cancellationToken)
        {
            var (usersId, achievements) = request;

            foreach (var userId in usersId)
            {
                foreach (var achievement in achievements)
                {
                    await _mediator.Send(new CheckAchievementInUserCommand(userId, achievement), cancellationToken);
                }
            }

            return new Unit();
        }
    }
}
