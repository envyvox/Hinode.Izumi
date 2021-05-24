using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.MarketService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Queries
{
    public record GetMarketUserRequestsInCategoryQuery(
            long UserId,
            MarketCategory Category)
        : IRequest<MarketRequestRecord[]>;

    public class GetMarketUserRequestsInCategoryHandler
        : IRequestHandler<GetMarketUserRequestsInCategoryQuery, MarketRequestRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetMarketUserRequestsInCategoryHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<MarketRequestRecord[]> Handle(GetMarketUserRequestsInCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var (userId, category) = request;
            return (await _con.GetConnection()
                    .QueryAsync<MarketRequestRecord>(@"
                        select * from market_requests
                        where user_id = @userId
                          and category = @category
                        order by id",
                        new {userId, category}))
                .ToArray();
        }
    }
}
