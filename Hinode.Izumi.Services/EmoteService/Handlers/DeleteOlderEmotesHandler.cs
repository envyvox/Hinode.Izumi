using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService.Commands;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.EmoteService.Handlers
{
    public class DeleteOlderEmotesHandler : IRequestHandler<DeleteOlderEmotesCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public DeleteOlderEmotesHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<Unit> Handle(DeleteOlderEmotesCommand request, CancellationToken cancellationToken)
        {
            await _con
                .GetConnection()
                .ExecuteAsync(@"
                    delete from emotes
                    where updated_at < @dateTime",
                    new {dateTime = request.DateTimeOffset});

            _cache.Remove(CacheExtensions.EmotesKey);

            return new Unit();
        }
    }
}
