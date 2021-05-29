using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CardService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Queries
{
    public record GetUserDeckQuery(long UserId) : IRequest<CardRecord[]>;

    public class GetUserDeckLengthHandler : IRequestHandler<GetUserDeckLengthQuery, int>
    {
        private readonly IConnectionManager _con;

        public GetUserDeckLengthHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<int> Handle(GetUserDeckLengthQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<int>(@"
                    select count(*) from user_decks
                    where user_id = @userId",
                    new {userId = request.UserId});
        }
    }
}
