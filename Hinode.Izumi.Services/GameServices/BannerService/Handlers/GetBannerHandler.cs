using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.BannerService.Queries;
using Hinode.Izumi.Services.GameServices.BannerService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.BannerService.Handlers
{
    public class GetBannerHandler : IRequestHandler<GetBannerQuery, BannerRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetBannerHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<BannerRecord> Handle(GetBannerQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.BannerKey, request.Id), out BannerRecord banner))
                return banner;

            banner = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<BannerRecord>(@"
                    select * from banners
                    where id = @bannerId",
                    new {bannerId = request.Id});

            if (banner is not null)
            {
                _cache.Set(string.Format(CacheExtensions.BannerKey, request.Id), banner,
                    CacheExtensions.DefaultCacheOptions);
                return banner;
            }

            await Task.FromException(new Exception(IzumiNullableMessage.Banner.Parse()));
            return null;
        }
    }
}
