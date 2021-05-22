using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.AlcoholService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.AlcoholService.Handlers
{
    public class GetAllAlcoholHandler : IRequestHandler<GetAllAlcoholQuery, AlcoholRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllAlcoholHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<AlcoholRecord[]> Handle(GetAllAlcoholQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<AlcoholRecord>(@"
                        select * from alcohols"))
                .ToArray();
        }
    }
}
