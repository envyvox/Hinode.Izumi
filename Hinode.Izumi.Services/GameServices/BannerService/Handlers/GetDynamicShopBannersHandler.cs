using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.BannerService.Queries;
using Hinode.Izumi.Services.GameServices.BannerService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Handlers
{
    public class GetDynamicShopBannersHandler : IRequestHandler<GetDynamicShopBannersQuery, BannerRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetDynamicShopBannersHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<BannerRecord[]> Handle(GetDynamicShopBannersQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<BannerRecord>(@"
                        select b.* from dynamic_shop_banners as dsb
                            inner join banners b
                                on b.id = dsb.banner_id"))
                .ToArray();
        }
    }
}
