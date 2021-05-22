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
    public class GetCraftingIngredientsHandler
        : IRequestHandler<GetCraftingIngredientsQuery, CraftingIngredientRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetCraftingIngredientsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<CraftingIngredientRecord[]> Handle(GetCraftingIngredientsQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<CraftingIngredientRecord>(@"
                        select * from crafting_ingredients
                        where crafting_id = @craftingId
                        order by ingredient_id",
                        new {craftingId = request.CraftingId}))
                .ToArray();
        }
    }
}
