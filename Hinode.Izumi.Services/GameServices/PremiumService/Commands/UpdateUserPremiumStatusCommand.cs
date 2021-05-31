using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.PremiumService.Commands
{
    public record UpdateUserPremiumStatusCommand(long UserId, bool Premium) : IRequest;

    public class UpdateUserPremiumStatusHandler : IRequestHandler<UpdateUserPremiumStatusCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public UpdateUserPremiumStatusHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<Unit> Handle(UpdateUserPremiumStatusCommand request, CancellationToken cancellationToken)
        {
            var (userId, premium) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set premium = @premium,
                        updated_at = now()
                    where id = @userId",
                    new {userId, premium});

            _cache.Set(string.Format(CacheExtensions.UserHasPremium, userId), premium,
                CacheExtensions.DefaultCacheOptions);

            return new Unit();
        }
    }
}
