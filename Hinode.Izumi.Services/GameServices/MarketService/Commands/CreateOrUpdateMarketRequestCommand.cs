using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Commands
{
    public record CreateOrUpdateMarketRequestCommand(
            long UserId,
            MarketCategory Category,
            long ItemId,
            long Price,
            long Amount,
            bool Selling)
        : IRequest;

    public class CreateOrUpdateMarketRequestHandler : IRequestHandler<CreateOrUpdateMarketRequestCommand>
    {
        private readonly IConnectionManager _con;

        public CreateOrUpdateMarketRequestHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(CreateOrUpdateMarketRequestCommand request, CancellationToken cancellationToken)
        {
            var (userId, category, itemId, price, amount, selling) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into market_requests(user_id, category, item_id, price, amount, selling)
                    values (@userId, @category, @itemId, @price, @amount, @selling)
                    on conflict (user_id, category, item_id) do update
                        set price = @price,
                            amount = @amount,
                            updated_at = now()",
                    new {userId, category, itemId, price, amount, selling});

            return new Unit();
        }
    }
}
