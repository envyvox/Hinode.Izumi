using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.BuildingService.Queries;
using Hinode.Izumi.Services.GameServices.BuildingService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Handlers
{
    public class GetUserBuildingsHandler : IRequestHandler<GetUserBuildingsQuery, BuildingRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserBuildingsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<BuildingRecord[]> Handle(GetUserBuildingsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<BuildingRecord>(@"
                        select b.* from user_buildings as ub
                            inner join buildings b
                                on b.id = ub.building_id
                        where ub.user_id = @userId
                        order by ub.building_id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
