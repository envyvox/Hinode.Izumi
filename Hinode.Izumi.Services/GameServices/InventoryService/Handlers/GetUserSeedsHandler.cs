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
    public class GetUserSeedsHandler : IRequestHandler<GetUserSeedsQuery, UserSeedRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserSeedsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserSeedRecord[]> Handle(GetUserSeedsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserSeedRecord>(@"
                        select us.*, s.name, s.season from user_seeds as us
                            inner join seeds s
                                on s.id = us.seed_id
                        where us.user_id = @userId
                        order by s.id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
