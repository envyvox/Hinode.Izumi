using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Handlers
{
    public class AddTitleToUsersHandler : IRequestHandler<AddTitleToUsersCommand>
    {
        private readonly IConnectionManager _con;

        public AddTitleToUsersHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddTitleToUsersCommand request, CancellationToken cancellationToken)
        {
            var (usersId, title) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_titles(user_id, title)
                    values (unnest(array[@usersId]), @title)
                    on conflict (user_id, title) do nothing",
                    new {usersId, title});

            return new Unit();
        }
    }
}
