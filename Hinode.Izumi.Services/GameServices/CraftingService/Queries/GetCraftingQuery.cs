using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CraftingService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.CraftingService.Queries
{
    public record GetCraftingQuery(long Id) : IRequest<CraftingRecord>;

    public class GetCraftingHandler : IRequestHandler<GetCraftingQuery, CraftingRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetCraftingHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<CraftingRecord> Handle(GetCraftingQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.CraftingKey, request.Id), out CraftingRecord crafting))
                return crafting;

            crafting = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CraftingRecord>(@"
                    select * from craftings
                    where id = @id",
                    new {id = request.Id});

            _cache.Set(string.Format(CacheExtensions.CraftingKey, request.Id), crafting,
                CacheExtensions.DefaultCacheOptions);

            return crafting;
        }
    }
}
