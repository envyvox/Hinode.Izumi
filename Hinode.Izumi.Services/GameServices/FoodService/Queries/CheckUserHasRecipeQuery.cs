using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FoodService.Queries
{
    public record CheckUserHasRecipeQuery(long UserId, long FoodId) : IRequest<bool>;

    public class CheckUserHasRecipeHandler : IRequestHandler<CheckUserHasRecipeQuery, bool>
    {
        private readonly IConnectionManager _con;

        public CheckUserHasRecipeHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<bool> Handle(CheckUserHasRecipeQuery request, CancellationToken cancellationToken)
        {
            var (userId, foodId) = request;

            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_recipes
                    where user_id = @userId
                      and food_id = @foodId",
                    new {userId, foodId});
        }
    }
}
