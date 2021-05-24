using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record AddTitleToUserCommand(long Id, Title Title) : IRequest;

    public class AddTitleToUserHandler : IRequestHandler<AddTitleToUserCommand>
    {
        private readonly IConnectionManager _con;

        public AddTitleToUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddTitleToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, title) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_titles(user_id, title)
                    values (@userId, @title)
                    on conflict (user_id, title) do nothing",
                    new {userId, title});

            return new Unit();
        }
    }
}
