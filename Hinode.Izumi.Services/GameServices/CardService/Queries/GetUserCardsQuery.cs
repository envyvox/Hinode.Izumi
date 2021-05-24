using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CardService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Queries
{
    public record GetUserCardsQuery(long UserId) : IRequest<CardRecord[]>;

    public class GetUserCardsHandler : IRequestHandler<GetUserCardsQuery, CardRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserCardsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<CardRecord[]> Handle(GetUserCardsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<CardRecord>(@"
                        select c.* from user_cards as uc
                            inner join cards c
                                on c.id = uc.card_id
                        where uc.user_id = @userId",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
