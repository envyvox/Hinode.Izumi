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
    public class GetProjectIngredientsHandler : IRequestHandler<GetProjectIngredientsQuery, ProjectIngredientRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetProjectIngredientsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<ProjectIngredientRecord[]> Handle(GetProjectIngredientsQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<ProjectIngredientRecord>(@"
                        select * from project_ingredients
                        where project_id = @projectId
                        order by ingredient_id",
                        new {projectId = request.ProjectId}))
                .ToArray();
        }
    }
}
