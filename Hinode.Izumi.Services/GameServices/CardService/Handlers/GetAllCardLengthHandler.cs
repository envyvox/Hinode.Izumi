using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CardService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Handlers
{
    public class GetAllCardLengthHandler : IRequestHandler<GetAllCardLengthQuery, long>
    {
        private readonly IConnectionManager _con;

        public GetAllCardLengthHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<long> Handle(GetAllCardLengthQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<long>(@"
                    select count(*) from cards");
        }
    }
}
