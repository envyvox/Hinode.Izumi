using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using Hinode.Izumi.Services.GameServices.IngredientService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Handlers
{
    public class GetAlcoholIngredientsHandler : IRequestHandler<GetAlcoholIngredientsQuery, AlcoholIngredientRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAlcoholIngredientsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<AlcoholIngredientRecord[]> Handle(GetAlcoholIngredientsQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<AlcoholIngredientRecord>(@"
                        select * from alcohol_ingredients
                        where alcohol_id = @alcoholId
                        order by ingredient_id",
                        new {alcoholId = request.AlcoholId}))
                .ToArray();
        }
    }
}
