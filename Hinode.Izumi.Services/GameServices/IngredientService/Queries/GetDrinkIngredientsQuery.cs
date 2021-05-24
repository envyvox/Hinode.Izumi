using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.IngredientService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Queries
{
    public record GetDrinkIngredientsQuery(long DrinkId) : IRequest<DrinkIngredientRecord[]>;

    public class GetDrinkIngredientsHandler : IRequestHandler<GetDrinkIngredientsQuery, DrinkIngredientRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetDrinkIngredientsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<DrinkIngredientRecord[]> Handle(GetDrinkIngredientsQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<DrinkIngredientRecord>(@"
                        select * from drink_ingredients
                        where drink_id = @drinkId
                        order by ingredient_id",
                        new {drinkId = request.DrinkId}))
                .ToArray();
        }
    }
}
