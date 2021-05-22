using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.AchievementService.Queries;
using Hinode.Izumi.Services.GameServices.AchievementService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Handlers
{
    public class GetAchievementByIdHandler : IRequestHandler<GetAchievementByIdQuery, AchievementRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetAchievementByIdHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<AchievementRecord> Handle(GetAchievementByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.AchievementIdKey, request.Id),
                out AchievementRecord achievement)) return achievement;

            achievement = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AchievementRecord>(@"
                    select * from achievements
                    where id = @id",
                    new {id = request.Id});

            _cache.Set(string.Format(CacheExtensions.AchievementIdKey, request.Id), achievement,
                CacheExtensions.DefaultCacheOptions);

            return achievement;
        }
    }
}
