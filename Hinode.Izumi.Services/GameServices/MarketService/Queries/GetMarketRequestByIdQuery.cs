using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.MarketService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Queries
{
    public record GetMarketRequestByIdQuery(long Id) : IRequest<MarketRequestRecord>;

    public class GetMarketRequestByIdHandler : IRequestHandler<GetMarketRequestByIdQuery, MarketRequestRecord>
    {
        private readonly IConnectionManager _con;

        public GetMarketRequestByIdHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<MarketRequestRecord> Handle(GetMarketRequestByIdQuery request,
            CancellationToken cancellationToken)
        {
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<MarketRequestRecord>(@"
                    select * from market_requests
                    where id = @requestId",
                    new {requestId = request.Id});

            if (res is not null) return res;

            await Task.FromException(new Exception(IzumiNullableMessage.MarketRequest.Parse()));
            return null;
        }
    }
}
