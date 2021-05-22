using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Handlers
{
    public class CheckAchievementInUsersHandler : IRequestHandler<CheckAchievementInUsersCommand>
    {
        private readonly IMediator _mediator;

        public CheckAchievementInUsersHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckAchievementInUsersCommand request, CancellationToken cancellationToken)
        {
            var (usersId, achievement) = request;

            foreach (var userId in usersId)
            {
                await _mediator.Send(new CheckAchievementInUserCommand(userId, achievement), cancellationToken);
            }

            return new Unit();
        }
    }
}
