using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CardService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Queries
{
    public record GetUserDeckLengthQuery(long UserId) : IRequest<int>;

    public class GetUserDeckHandler : IRequestHandler<GetUserDeckQuery, CardRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserDeckHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<CardRecord[]> Handle(GetUserDeckQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<CardRecord>(@"
                        select c.* from user_decks as ud
                            inner join cards c
                                on c.id = ud.card_id
                        where ud.user_id = @userId",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
