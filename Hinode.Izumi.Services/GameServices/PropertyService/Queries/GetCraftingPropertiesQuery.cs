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
    public record GetCraftingPropertiesQuery(
            long CraftingId,
            CraftingProperty Property)
        : IRequest<CraftingPropertyRecord>;

    public class GetCraftingPropertiesHandler : IRequestHandler<GetCraftingPropertiesQuery, CraftingPropertyRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetCraftingPropertiesHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<CraftingPropertyRecord> Handle(GetCraftingPropertiesQuery request,
            CancellationToken cancellationToken)
        {
            var (craftingId, property) = request;

            if (_cache.TryGetValue(string.Format(CacheExtensions.CraftingPropertyKey, craftingId, property),
                out CraftingPropertyRecord properties)) return properties;

            properties = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CraftingPropertyRecord>(@"
                    select * from crafting_properties
                    where crafting_id = @craftingId
                      and property = @property",
                    new {craftingId, property});

            _cache.Set(string.Format(CacheExtensions.CraftingPropertyKey, craftingId, property), properties,
                CacheExtensions.DefaultCacheOptions);

            return properties;
        }
    }
}
