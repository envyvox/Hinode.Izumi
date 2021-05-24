using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Queries
{
    public record CheckUserHasBannerQuery(long UserId, long BannerId) : IRequest<bool>;

    public class CheckUserHasBannerHandler : IRequestHandler<CheckUserHasBannerQuery, bool>
    {
        private readonly IConnectionManager _con;

        public CheckUserHasBannerHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<bool> Handle(CheckUserHasBannerQuery request, CancellationToken cancellationToken)
        {
            var (userId, bannerId) = request;

            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_banners
                    where user_id = @userId
                      and banner_id = @bannerId",
                    new {userId, bannerId});
        }
    }
}
