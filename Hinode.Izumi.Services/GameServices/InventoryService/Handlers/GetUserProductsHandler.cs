using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Handlers
{
    public class GetUserProductsHandler : IRequestHandler<GetUserProductsQuery, UserProductRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserProductsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserProductRecord[]> Handle(GetUserProductsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserProductRecord>(@"
                        select up.*, p.name from user_products as up
                            inner join products p
                                on p.id = up.product_id
                        where up.user_id = @userId
                        order by p.id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
