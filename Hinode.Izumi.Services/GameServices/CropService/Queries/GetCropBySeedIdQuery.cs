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
    public record GetCropBySeedIdQuery(long SeedId) : IRequest<CropRecord>;

    public class GetCropBySeedIdHandler : IRequestHandler<GetCropBySeedIdQuery, CropRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetCropBySeedIdHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<CropRecord> Handle(GetCropBySeedIdQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.CropBySeedKey, request.SeedId), out CropRecord crop))
                return crop;

            crop = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CropRecord>(@"
                    select c.*, s.season from crops as c
                        inner join seeds s
                            on s.id = c.seed_id
                    where c.seed_id = @seedId",
                    new {seedId = request.SeedId});

            _cache.Set(string.Format(CacheExtensions.CropBySeedKey, request.SeedId), crop,
                CacheExtensions.DefaultCacheOptions);

            return crop;
        }
    }
}
