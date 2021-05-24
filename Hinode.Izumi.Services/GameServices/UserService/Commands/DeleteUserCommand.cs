using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record DeleteUserCommand(long Id) : IRequest;

    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public DeleteUserHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserRecord>(@"
                    delete from users
                    where id = @userId
                    returning id, name",
                    new {userId = request.Id});

            _cache.Remove(string.Format(CacheExtensions.UserWithIdCheckKey, user.Id));
            _cache.Remove(string.Format(CacheExtensions.UserWithNameCheckKey, user.Name));

            _cache.Set(string.Format(CacheExtensions.UserWithIdCheckKey, user.Id), false,
                CacheExtensions.DefaultCacheOptions);
            _cache.Set(string.Format(CacheExtensions.UserWithNameCheckKey, user.Name), false,
                CacheExtensions.DefaultCacheOptions);

            return new Unit();
        }
    }
}
