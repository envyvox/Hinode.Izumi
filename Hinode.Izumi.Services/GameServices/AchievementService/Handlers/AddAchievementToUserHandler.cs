using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Handlers
{
    public class AddAchievementToUserHandler : IRequestHandler<AddAchievementToUserCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;

        public AddAchievementToUserHandler(IConnectionManager con, IMediator mediator, IMemoryCache cache)
        {
            _con = con;
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<Unit> Handle(AddAchievementToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, achievementId) = request;

            await _con
                .GetConnection()
                .ExecuteAsync(@"
                    insert into user_achievements(user_id, achievement_id)
                    values (@userId, @achievementId)
                    on conflict (user_id, achievement_id) do nothing",
                    new {userId, achievementId});

            _cache.Set(string.Format(CacheExtensions.UserAchievementKey, userId, achievementId), true,
                CacheExtensions.DefaultCacheOptions);

            await _mediator.Send(new AddAchievementRewardToUserCommand(userId, achievementId), cancellationToken);

            return new Unit();
        }
    }
}
