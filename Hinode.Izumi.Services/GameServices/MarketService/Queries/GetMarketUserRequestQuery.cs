using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.MarketService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Queries
{
    public record GetMarketUserRequestQuery(
            long UserId,
            MarketCategory Category,
            long ItemId)
        : IRequest<MarketRequestRecord>;

    public class GetMarketUserRequestHandler : IRequestHandler<GetMarketUserRequestQuery, MarketRequestRecord>
    {
        private readonly IConnectionManager _con;

        public GetMarketUserRequestHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<MarketRequestRecord> Handle(GetMarketUserRequestQuery request,
            CancellationToken cancellationToken)
        {
            var (userId, category, itemId) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<MarketRequestRecord>(@"
                    select * from market_requests
                    where user_id = @userId
                      and category = @category
                      and item_id = @itemId
                    order by id",
                    new {userId, category, itemId});
        }
    }
}
