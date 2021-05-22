using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.AchievementService.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Handlers
{
    public class CheckUserHasAchievementHandler : IRequestHandler<CheckUserHasAchievementQuery, bool>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public CheckUserHasAchievementHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<bool> Handle(CheckUserHasAchievementQuery request, CancellationToken cancellationToken)
        {
            var (userId, achievementId) = request;

            if (_cache.TryGetValue(string.Format(CacheExtensions.UserAchievementKey, userId, achievementId),
                out bool hasAchievement)) return hasAchievement;

            hasAchievement = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_achievements
                    where user_id = @userId
                      and achievement_id = @achievementId",
                    new {userId, achievementId});

            _cache.Set(string.Format(CacheExtensions.UserAchievementKey, userId, achievementId), hasAchievement,
                CacheExtensions.DefaultCacheOptions);

            return hasAchievement;
        }
    }
}
