using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.UserService.Queries
{
    public record CheckUserByIdQuery(long Id) : IRequest<bool>;

    public class CheckUserByIdHandler : IRequestHandler<CheckUserByIdQuery, bool>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public CheckUserByIdHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<bool> Handle(CheckUserByIdQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.UserWithIdCheckKey, request.Id), out bool check))
                return check;

            check = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from users
                    where id = @userId",
                    new {userId = request.Id});

            _cache.Set(string.Format(CacheExtensions.UserWithIdCheckKey, request.Id), check,
                CacheExtensions.DefaultCacheOptions);

            return check;
        }
    }
}
