using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CardService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Handlers
{
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
