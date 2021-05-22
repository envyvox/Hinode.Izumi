using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ProjectService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ProjectService.Handlers
{
    public class CheckUserHasProjectHandler : IRequestHandler<CheckUserHasProjectQuery, bool>
    {
        private readonly IConnectionManager _con;

        public CheckUserHasProjectHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<bool> Handle(CheckUserHasProjectQuery request, CancellationToken cancellationToken)
        {
            var (userId, projectId) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_projects
                    where user_id = @userId
                      and project_id = @projectId",
                    new {userId, projectId});
        }
    }
}
