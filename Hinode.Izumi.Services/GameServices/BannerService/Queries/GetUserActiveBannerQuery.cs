using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.BannerService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Queries
{
    public record GetUserActiveBannerQuery(long UserId) : IRequest<BannerRecord>;

    public class GetUserActiveBannerHandler : IRequestHandler<GetUserActiveBannerQuery, BannerRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserActiveBannerHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<BannerRecord> Handle(GetUserActiveBannerQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<BannerRecord>(@"
                    select b.* from user_banners as ub
                        inner join banners b
                            on b.id = ub.banner_id
                    where ub.user_id = @userId
                      and active = true",
                    new {userId = request.UserId});
        }
    }
}
