using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.AlcoholService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.AlcoholService.Queries
{
    public record GetAlcoholQuery(long Id) : IRequest<AlcoholRecord>;

    public class GetAlcoholHandler : IRequestHandler<GetAlcoholQuery, AlcoholRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetAlcoholHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<AlcoholRecord> Handle(GetAlcoholQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.AlcoholKey, request.Id), out AlcoholRecord alcohol))
                return alcohol;

            alcohol = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AlcoholRecord>(@"
                    select * from alcohols
                    where id = @id",
                    new {id = request.Id});

            _cache.Set(string.Format(CacheExtensions.AlcoholKey, request.Id), alcohol,
                CacheExtensions.DefaultCacheOptions);

            return alcohol;
        }
    }
}
