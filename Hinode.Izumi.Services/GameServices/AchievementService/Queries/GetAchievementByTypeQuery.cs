using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.AchievementService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Queries
{
    public record GetAchievementByTypeQuery(Achievement Type) : IRequest<AchievementRecord>;

    public class GetAchievementByTypeHandler : IRequestHandler<GetAchievementByTypeQuery, AchievementRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetAchievementByTypeHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<AchievementRecord> Handle(GetAchievementByTypeQuery request,
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.AchievementTypeKey, request.Type),
                out AchievementRecord achievement)) return achievement;

            achievement = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AchievementRecord>(@"
                    select * from achievements
                    where type = @type",
                    new {type = request.Type});

            _cache.Set(string.Format(CacheExtensions.AchievementTypeKey, request.Type), achievement,
                CacheExtensions.DefaultCacheOptions);

            return achievement;
        }
    }
}
