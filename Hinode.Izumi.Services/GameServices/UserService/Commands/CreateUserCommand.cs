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
    public record CreateUserCommand(long Id, string Name) : IRequest;

    public class CreateUserHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public CreateUserHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, name) = request;

            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserRecord>(@"
                    insert into users(id, name)
                    values (@userId, @name)
                    returning *",
                    new {userId, name});

            _cache.Set(string.Format(CacheExtensions.UserWithIdCheckKey, userId), true,
                CacheExtensions.DefaultCacheOptions);
            _cache.Set(string.Format(CacheExtensions.UserWithNameCheckKey, name), true,
                CacheExtensions.DefaultCacheOptions);

            return new Unit();
        }
    }
}
