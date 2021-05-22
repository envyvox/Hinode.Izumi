using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.BannerService.Queries;
using Hinode.Izumi.Services.GameServices.BannerService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Handlers
{
    public class GetDynamicShopBannerHandler : IRequestHandler<GetDynamicShopBannerQuery, BannerRecord>
    {
        private readonly IConnectionManager _con;

        public GetDynamicShopBannerHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<BannerRecord> Handle(GetDynamicShopBannerQuery request, CancellationToken cancellationToken)
        {
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<BannerRecord>(@"
                    select b.* from dynamic_shop_banners as dsb
                        inner join banners b
                            on b.id = dsb.banner_id
                    where dsb.banner_id = @bannerId",
                    new {bannerId = request.BannerId});

            if (res is null)
                await Task.FromException(new Exception(IzumiNullableMessage.DynamicShopBanner.Parse()));

            return res;
        }
    }
}
