using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using Hinode.Izumi.Services.GameServices.LocationService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Handlers
{
    public class GetUserMovementHandler : IRequestHandler<GetUserMovementQuery, MovementRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserMovementHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<MovementRecord> Handle(GetUserMovementQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<MovementRecord>(@"
                    select * from movements
                    where user_id = @userId",
                    new {userId = request.UserId});
        }
    }
}
