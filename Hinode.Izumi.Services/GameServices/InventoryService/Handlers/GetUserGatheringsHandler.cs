using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Handlers
{
    public class GetUserGatheringsHandler : IRequestHandler<GetUserGatheringsQuery, UserGatheringRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserGatheringsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserGatheringRecord[]> Handle(GetUserGatheringsQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserGatheringRecord>(@"
                        select ug.*, g.name from user_gatherings as ug
                            inner join gatherings g
                                on g.id = ug.gathering_id
                        where ug.user_id = @userId
                        order by g.id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
