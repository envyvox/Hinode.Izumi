using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ProjectService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ProjectService.Handlers
{
    public class RemoveProjectFromUserHandler : IRequestHandler<RemoveProjectFromUserCommand>
    {
        private readonly IConnectionManager _con;

        public RemoveProjectFromUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(RemoveProjectFromUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, projectId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_projects
                    where user_id = @userId
                      and project_id = @projectId",
                    new {userId, projectId});

            return new Unit();
        }
    }
}
