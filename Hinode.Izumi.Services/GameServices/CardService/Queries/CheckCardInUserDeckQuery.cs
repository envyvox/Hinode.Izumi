using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Queries
{
    public record CheckCardInUserDeckQuery(long UserId, long CardId) : IRequest<bool>;

    public class CheckCardInUserDeckHandler : IRequestHandler<CheckCardInUserDeckQuery, bool>
    {
        private readonly IConnectionManager _con;

        public CheckCardInUserDeckHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<bool> Handle(CheckCardInUserDeckQuery request, CancellationToken cancellationToken)
        {
            var (userId, cardId) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_decks
                    where user_id = @userId
                      and card_id = @card_id",
                    new {userId, cardId});
        }
    }
}
