using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.MarketService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Queries
{
    public record GetMarketUserRequestsQuery(long UserId) : IRequest<MarketRequestRecord[]>;

    public class GetMarketUserRequestsHandler : IRequestHandler<GetMarketUserRequestsQuery, MarketRequestRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetMarketUserRequestsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<MarketRequestRecord[]> Handle(GetMarketUserRequestsQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con
                    .GetConnection()
                    .QueryAsync<MarketRequestRecord>(@"
                        select * from market_requests
                        where user_id = @userId
                        order by id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
