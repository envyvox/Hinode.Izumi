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
    public class GetUserDrinksHandler : IRequestHandler<GetUserDrinksQuery, UserDrinkRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserDrinksHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserDrinkRecord[]> Handle(GetUserDrinksQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserDrinkRecord>(@"
                        select ud.*, d.name from user_drinks as ud
                            inner join drinks d
                                on d.id = ud.drink_id
                        where ud.user_id = @userId
                        order by d.id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
