using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserAlcoholsQuery(long UserId) : IRequest<UserAlcoholRecord[]>;

    public class GetUserAlcoholsHandler : IRequestHandler<GetUserAlcoholsQuery, UserAlcoholRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserAlcoholsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserAlcoholRecord[]> Handle(GetUserAlcoholsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserAlcoholRecord>(@"
                        select ua.*, a.name from user_alcohols as ua
                            inner join alcohols a
                                on a.id = ua.alcohol_id
                        where ua.user_id = @userId
                        order by a.id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
