using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.MarketService.Queries;
using Hinode.Izumi.Services.GameServices.MarketService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Handlers
{
    public class GetMarketRequestByParamsHandler
        : IRequestHandler<GetMarketRequestByParamsQuery, MarketRequestRecord[]>
    {
        private readonly IConnectionManager _con;
        private readonly ILocalizationService _local;

        public GetMarketRequestByParamsHandler(IConnectionManager con, ILocalizationService local)
        {
            _con = con;
            _local = local;
        }

        public async Task<MarketRequestRecord[]> Handle(GetMarketRequestByParamsQuery request,
            CancellationToken cancellationToken)
        {
            var (category, selling, namePattern) = request;

            long? itemId = namePattern is null
                // если название пустое, то нам не нужно искать заявки определенного предмета
                ? null
                // если не пустое - нужно получить id предмета, который мы будем искать на рынке
                : (await _local.GetLocalizationByLocalizedWord(category, namePattern)).ItemId;

            return (await _con.GetConnection()
                    .QueryAsync<MarketRequestRecord>(@"
                        select * from market_requests
                        where category = @category
                          and (
                              @itemId is null
                                  or item_id = @itemId
                              )
                          and selling = @selling
                        order by price desc
                        limit 5",
                        new {category, itemId, selling}))
                .ToArray();
        }
    }
}
