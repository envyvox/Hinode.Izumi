using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.PremiumService.Queries
{
    public record GetUserCommandColorQuery(long UserId) : IRequest<string>;

    public class GetUserCommandColorHandler : IRequestHandler<GetUserCommandColorQuery, string>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetUserCommandColorHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<string> Handle(GetUserCommandColorQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.UserCommandColor, request.UserId),
                out string commandColor)) return commandColor;

            commandColor = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<string>(@"
                    select command_color from user_premium_propertieses
                    where user_id = @userId",
                    new {userId = request.UserId});

            _cache.Set(string.Format(CacheExtensions.UserCommandColor, request.UserId), commandColor,
                CacheExtensions.DefaultCacheOptions);

            return commandColor;
        }
    }
}
