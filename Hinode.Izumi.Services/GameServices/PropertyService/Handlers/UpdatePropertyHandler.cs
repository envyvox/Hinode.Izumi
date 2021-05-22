using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.PropertyService.Commands;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;


namespace Hinode.Izumi.Services.GameServices.PropertyService.Handlers
{
    public class UpdatePropertyHandler : IRequestHandler<UpdatePropertyCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public UpdatePropertyHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<Unit> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var (property, newValue) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update world_properties
                    set value = @newValue,
                        updated_at = now()
                    where property = @property",
                    new {property, newValue});

            _cache.Set(string.Format(CacheExtensions.PropertyKey, property), newValue,
                CacheExtensions.DefaultCacheOptions);

            return new Unit();
        }
    }
}
