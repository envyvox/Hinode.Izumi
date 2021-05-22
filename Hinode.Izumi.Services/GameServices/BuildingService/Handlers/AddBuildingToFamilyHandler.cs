using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.BuildingService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Handlers
{
    public class AddBuildingToFamilyHandler : IRequestHandler<AddBuildingToFamilyCommand>
    {
        private readonly IConnectionManager _con;

        public AddBuildingToFamilyHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddBuildingToFamilyCommand request, CancellationToken cancellationToken)
        {
            var (familyId, type) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into family_buildings(family_id, building_id)
                    values (@familyId, (
                        select id from buildings
                        where type = @type
                    ))
                    on conflict (family_id, building_id) do nothing",
                    new {familyId, type});

            return new Unit();
        }
    }
}
