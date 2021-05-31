using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.PremiumService.Commands
{
    public record UpdateUserCommandColorCommand(long UserId, string Color) : IRequest;

    public class UpdateUserCommandColorHandler : IRequestHandler<UpdateUserCommandColorCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public UpdateUserCommandColorHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<Unit> Handle(UpdateUserCommandColorCommand request, CancellationToken cancellationToken)
        {
            var (userId, color) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_premium_propertieses
                    set command_color = @color,
                        updated_at = now()
                    where user_id = @userId",
                    new {userId, color});

            _cache.Set(string.Format(CacheExtensions.UserCommandColor, userId), color,
                CacheExtensions.DefaultCacheOptions);

            return new Unit();
        }
    }
}
