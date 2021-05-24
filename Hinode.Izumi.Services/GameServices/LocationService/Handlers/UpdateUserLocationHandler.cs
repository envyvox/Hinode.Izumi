using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.LocationService.Handlers
{
    public class UpdateUserLocationHandler : IRequestHandler<UpdateUserLocationCommand>
    {
        private readonly IConnectionManager _con;

        public UpdateUserLocationHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(UpdateUserLocationCommand request, CancellationToken cancellationToken)
        {
            var (userId, location) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set location = @location,
                        updated_at = now()
                    where id = @userId",
                    new {userId, location});

            return new Unit();
        }
    }
}
