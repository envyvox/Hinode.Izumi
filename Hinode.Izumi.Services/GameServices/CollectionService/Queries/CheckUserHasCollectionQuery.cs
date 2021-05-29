using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CollectionService.Queries
{
    public record CheckUserHasCollectionQuery(long UserId, CollectionCategory Category, long ItemId) : IRequest<bool>;

    public class CheckUserHasCollectionHandler : IRequestHandler<CheckUserHasCollectionQuery, bool>
    {
        private readonly IConnectionManager _con;

        public CheckUserHasCollectionHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<bool> Handle(CheckUserHasCollectionQuery request, CancellationToken cancellationToken)
        {
            var (userId, category, itemId) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_collections
                    where user_id = @userId
                      and category = @category
                      and item_id = @itemId",
                    new {userId, category, itemId});
        }
    }
}
