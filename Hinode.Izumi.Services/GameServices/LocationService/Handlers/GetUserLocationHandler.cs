using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Handlers
{
    public class GetUserLocationHandler : IRequestHandler<GetUserLocationQuery, Location>
    {
        private readonly IConnectionManager _con;

        public GetUserLocationHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Location> Handle(GetUserLocationQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<Location>(@"
                    select location from users
                    where id = @userId",
                    new {userId = request.UserId});
        }
    }
}
