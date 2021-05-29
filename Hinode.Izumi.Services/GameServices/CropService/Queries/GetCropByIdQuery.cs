using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CropService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.CropService.Queries
{
    public record GetCropByIdQuery(long Id) : IRequest<CropRecord>;

    public class GetCropByIdHandler : IRequestHandler<GetCropByIdQuery, CropRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetCropByIdHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<CropRecord> Handle(GetCropByIdQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.CropByIdKey, request.Id), out CropRecord crop))
                return crop;

            crop = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CropRecord>(@"
                    select c.*, s.season from crops as c
                        inner join seeds s
                            on s.id = c.seed_id
                    where c.id = @id",
                    new {id = request.Id});

            _cache.Set(string.Format(CacheExtensions.CropByIdKey, request.Id), crop,
                CacheExtensions.DefaultCacheOptions);

            return crop;
        }
    }
}
