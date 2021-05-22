using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.MarketService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Handlers
{
    public class UpdateOrDeleteMarketRequestHandler : IRequestHandler<UpdateOrDeleteMarketRequestCommand>
    {
        private readonly IConnectionManager _con;

        public UpdateOrDeleteMarketRequestHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(UpdateOrDeleteMarketRequestCommand request, CancellationToken cancellationToken)
        {
            var (category, itemId, marketAmount, amount) = request;

            var query = marketAmount == amount
                // если это был последний товар, то заявку нужно удалить
                ? @"delete from market_requests
                        where category = @category
                          and item_id = @itemId"
                // если нет - обновить
                : @"update market_requests
                        set amount = amount - @amount,
                            updated_at = now()
                    where category = @category
                      and item_id = @itemId";

            await _con.GetConnection()
                .ExecuteAsync(query, new {category, itemId, amount});

            return new Unit();
        }
    }
}
