using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.BuildingService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Handlers
{
    public class AddBuildingToUserHandler : IRequestHandler<AddBuildingToUserCommand>
    {
        private readonly IConnectionManager _con;

        public AddBuildingToUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddBuildingToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, type) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_buildings(user_id, building_id)
                    values (@userId, (
                        select id from buildings
                        where type = @type
                    ))
                    on conflict (user_id, building_id) do nothing",
                    new {userId, type});

            return new Unit();
        }
    }
}
