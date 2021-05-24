using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Handlers
{
    public class AddUserMovementHandler : IRequestHandler<CreateUserMovementCommand>
    {
        private readonly IConnectionManager _con;

        public AddUserMovementHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(CreateUserMovementCommand request, CancellationToken cancellationToken)
        {
            var (userId, departure, destination, arrival) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into movements(user_id, departure, destination, arrival)
                    values (@userId, @departure, @destination, @arrival)
                    on conflict (user_id) do nothing",
                    new {userId, departure, destination, arrival});

            return new Unit();
        }
    }
}
