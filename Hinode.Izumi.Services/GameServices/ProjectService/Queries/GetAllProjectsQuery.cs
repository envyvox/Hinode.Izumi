using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ProjectService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ProjectService.Queries
{
    public record GetAllProjectsQuery : IRequest<ProjectRecord[]>;

    public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, ProjectRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllProjectsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<ProjectRecord[]> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<ProjectRecord>(@"
                        select * from projects
                        order by id"))
                .ToArray();
        }
    }
}
