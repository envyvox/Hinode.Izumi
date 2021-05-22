using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Handlers
{
    public class DeleteUserMovementHandler : IRequestHandler<DeleteUserMovementCommand>
    {
        private readonly IConnectionManager _con;

        public DeleteUserMovementHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(DeleteUserMovementCommand request, CancellationToken cancellationToken)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from movements
                    where user_id = @userId",
                    new {userId = request.UserId});

            return new Unit();
        }
    }
}
