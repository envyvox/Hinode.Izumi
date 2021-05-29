using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.IngredientService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Queries
{
    public record GetFoodIngredientsQuery(long FoodId) : IRequest<FoodIngredientRecord[]>;

    public class GetFoodIngredientsHandler : IRequestHandler<GetFoodIngredientsQuery, FoodIngredientRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetFoodIngredientsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FoodIngredientRecord[]> Handle(GetFoodIngredientsQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<FoodIngredientRecord>(@"
                        select * from food_ingredients
                        where food_id = @foodId
                        order by ingredient_id",
                        new {foodId = request.FoodId}))
                .ToArray();
        }
    }
}
