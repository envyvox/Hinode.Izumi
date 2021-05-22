using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.EmoteService.Handlers
{
    public class GetEmotesHandler : IRequestHandler<GetEmotesQuery, Dictionary<string, EmoteRecord>>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetEmotesHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<Dictionary<string, EmoteRecord>> Handle(GetEmotesQuery request,
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(CacheExtensions.EmotesKey, out Dictionary<string, EmoteRecord> emotes))
                return emotes;

            emotes = (await _con.GetConnection()
                    .QueryAsync<EmoteRecord>(@"
                        select * from emotes"))
                .ToDictionary(x => x.Name);

            _cache.Set(CacheExtensions.EmotesKey, emotes, CacheExtensions.DefaultCacheOptions);

            return emotes;
        }
    }
}
