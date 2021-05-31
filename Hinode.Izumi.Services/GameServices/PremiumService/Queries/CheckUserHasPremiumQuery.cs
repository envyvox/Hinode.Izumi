using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.PremiumService.Queries
{
    public record CheckUserHasPremiumQuery(long UserId) : IRequest<bool>;

    public class CheckUserHasPremiumHandler : IRequestHandler<CheckUserHasPremiumQuery, bool>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public CheckUserHasPremiumHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<bool> Handle(CheckUserHasPremiumQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.UserHasPremium, request.UserId), out bool hasPremium))
                return hasPremium;

            hasPremium = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select premium from users
                    where id = @userId",
                    new {userId = request.UserId});

            _cache.Set(string.Format(CacheExtensions.UserHasPremium, request.UserId), hasPremium,
                CacheExtensions.DefaultCacheOptions);

            return hasPremium;
        }
    }
}
