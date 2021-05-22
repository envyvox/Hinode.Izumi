using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.UserService.Handlers
{
    public class CheckUserByNameHandler : IRequestHandler<CheckUserByNameQuery, bool>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public CheckUserByNameHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<bool> Handle(CheckUserByNameQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.UserWithNameCheckKey, request.Name), out bool check))
                return check;

            check = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from users
                    where name = @name",
                    new {name = request.Name});

            _cache.Set(string.Format(CacheExtensions.UserWithNameCheckKey, request.Name), check,
                CacheExtensions.DefaultCacheOptions);

            return check;
        }
    }
}
