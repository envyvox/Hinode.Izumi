using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FoodService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FoodService.Queries
{
    public record GetUserRecipesQuery(long UserId) : IRequest<FoodRecord[]>;

    public class GetUserRecipesHandler : IRequestHandler<GetUserRecipesQuery, FoodRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserRecipesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FoodRecord[]> Handle(GetUserRecipesQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<FoodRecord>(@"
                        select f.* from user_recipes as uc
                            inner join foods f
                                on f.id = uc.food_id
                        where uc.user_id = @userId
                        order by f.id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
