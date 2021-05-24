using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.BannerService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BannerService.Queries
{
    public record GetUserBannersQuery(long UserId) : IRequest<BannerRecord[]>;

    public class GetUserBannersHandler : IRequestHandler<GetUserBannersQuery, BannerRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserBannersHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<BannerRecord[]> Handle(GetUserBannersQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<BannerRecord>(@"
                        select * from user_banners as ub
                            inner join banners b
                                on b.id = ub.banner_id
                        where ub.user_id = @userId
                        order by b.id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
