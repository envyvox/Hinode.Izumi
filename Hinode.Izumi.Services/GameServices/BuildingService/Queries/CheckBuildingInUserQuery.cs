using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Queries
{
    public record CheckBuildingInUserQuery(long UserId, Building Type) : IRequest<bool>;

    public class CheckBuildingInUserHandler : IRequestHandler<CheckBuildingInUserQuery, bool>
    {
        private readonly IConnectionManager _con;

        public CheckBuildingInUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<bool> Handle(CheckBuildingInUserQuery request, CancellationToken cancellationToken)
        {
            var (userId, type) = request;

            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_buildings
                    where user_id = @userId
                      and building_id = (
                          select id from buildings
                          where type = @type
                          )",
                    new {userId, type});
        }
    }
}
