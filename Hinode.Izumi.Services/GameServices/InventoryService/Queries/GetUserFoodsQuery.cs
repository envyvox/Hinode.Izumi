using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserFoodsQuery(long UserId) : IRequest<UserFoodRecord[]>;

    public class GetUserFoodsHandler : IRequestHandler<GetUserFoodsQuery, UserFoodRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserFoodsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserFoodRecord[]> Handle(GetUserFoodsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserFoodRecord>(@"
                        select uf.*, f.name, f.mastery from user_foods as uf
                            inner join foods f
                                on f.id = uf.food_id
                        where uf.user_id = @userId
                        order by f.id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
