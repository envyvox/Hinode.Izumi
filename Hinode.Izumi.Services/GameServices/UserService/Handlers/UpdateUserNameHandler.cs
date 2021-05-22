using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Handlers
{
    public class UpdateUserNameHandler : IRequestHandler<UpdateUserNameCommand>
    {
        private readonly IConnectionManager _con;

        public UpdateUserNameHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
        {
            var (userId, newName) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set name = @newName,
                        updated_at = now()
                    where id = @userId",
                    new {userId, newName});

            return new Unit();
        }
    }
}
