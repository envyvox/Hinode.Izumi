using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserFishesQuery(long UserId) : IRequest<UserFishRecord[]>;

    public class GetUserFishesHandler : IRequestHandler<GetUserFishesQuery, UserFishRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserFishesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserFishRecord[]> Handle(GetUserFishesQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserFishRecord>(@"
                        select uf.*, f.name, f.rarity, f.seasons, f.price from user_fishes as uf
                            inner join fishes f
                                on f.id = uf.fish_id
                        where uf.user_id = @userId
                        order by f.id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
