using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ProjectService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ProjectService.Handlers
{
    public class AddProjectToUserHandler : IRequestHandler<AddProjectToUserCommand>
    {
        private readonly IConnectionManager _con;

        public AddProjectToUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddProjectToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, projectId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_projects(user_id, project_id)
                    values (@userId, @projectId)
                    on conflict (user_id, project_id) do nothing",
                    new {userId, projectId});

            return new Unit();
        }
    }
}
