using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.BannerService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Queries
{
    public record GetUserBannerQuery(long UserId, long BannerId) : IRequest<BannerRecord>;

    public class GetUserBannerHandler : IRequestHandler<GetUserBannerQuery, BannerRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserBannerHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<BannerRecord> Handle(GetUserBannerQuery request, CancellationToken cancellationToken)
        {
            var (userId, bannerId) = request;
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<BannerRecord>(@"
                    select b.* from user_banners as ub
                        inner join banners b
                            on b.id = ub.banner_id
                    where ub.user_id = @userId
                      and ub.banner_id = @bannerId",
                    new {userId, bannerId});

            if (res is null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserBanner.Parse()));

            return res;
        }
    }
}
