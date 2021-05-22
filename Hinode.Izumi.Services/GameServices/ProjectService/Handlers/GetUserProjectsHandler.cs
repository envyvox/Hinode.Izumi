using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ProjectService.Queries;
using Hinode.Izumi.Services.GameServices.ProjectService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ProjectService.Handlers
{
    public class GetUserProjectsHandler : IRequestHandler<GetUserProjectsQuery, ProjectRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserProjectsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<ProjectRecord[]> Handle(GetUserProjectsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<ProjectRecord>(@"
                        select p.* from user_projects as up
                            inner join projects p
                                on p.id = up.project_id
                        where up.user_id = @userId",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
