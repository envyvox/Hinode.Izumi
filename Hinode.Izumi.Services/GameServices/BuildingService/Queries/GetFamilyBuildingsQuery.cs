using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.BuildingService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Queries
{
    public record GetFamilyBuildingsQuery(long FamilyId) : IRequest<BuildingRecord[]>;

    public class GetFamilyBuildingsHandler : IRequestHandler<GetFamilyBuildingsQuery, BuildingRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetFamilyBuildingsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<BuildingRecord[]> Handle(GetFamilyBuildingsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<BuildingRecord>(@"
                        select b.* from family_buildings as fb
                            inner join buildings b
                                on b.id = fb.building_id
                        where fb.family_id = @familyId
                        order by fb.building_id",
                        new {familyId = request.FamilyId}))
                .ToArray();
        }
    }
}
