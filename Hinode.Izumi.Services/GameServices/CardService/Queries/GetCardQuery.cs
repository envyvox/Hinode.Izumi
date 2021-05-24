using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CardService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.CardService.Queries
{
    public record GetCardQuery(long Id) : IRequest<CardRecord>;

    public class GetCardHandler : IRequestHandler<GetCardQuery, CardRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetCardHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<CardRecord> Handle(GetCardQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.CardKey, request.Id), out CardRecord card))
                return card;

            card = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CardRecord>(@"
                    select * from cards
                    where id = @id",
                    new {id = request.Id});

            if (card is not null)
            {
                _cache.Set(string.Format(CacheExtensions.CardKey, request.Id), card,
                    CacheExtensions.DefaultCacheOptions);
                return card;
            }

            await Task.FromException(new Exception(IzumiNullableMessage.CardById.Parse()));
            return null;
        }
    }
}
