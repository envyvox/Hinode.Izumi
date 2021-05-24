using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.PropertyService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.PropertyService.Queries
{
    public record GetMasteryPropertiesQuery(MasteryProperty Property) : IRequest<MasteryPropertyRecord>;

    public class GetMasteryPropertyHandler : IRequestHandler<GetMasteryPropertiesQuery, MasteryPropertyRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetMasteryPropertyHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<MasteryPropertyRecord> Handle(GetMasteryPropertiesQuery request,
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.MasteryPropertyKey, request.Property),
                out MasteryPropertyRecord properties)) return properties;

            properties = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<MasteryPropertyRecord>(@"
                    select * from mastery_properties
                    where property = @property",
                    new {property = request.Property});

            _cache.Set(string.Format(CacheExtensions.MasteryPropertyKey, request.Property), properties,
                CacheExtensions.DefaultCacheOptions);

            return properties;
        }
    }
}
