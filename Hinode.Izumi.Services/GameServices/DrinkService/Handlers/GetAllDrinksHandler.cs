using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.DrinkService.Queries;
using Hinode.Izumi.Services.GameServices.DrinkService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.DrinkService.Handlers
{
    public class GetAllDrinksHandler : IRequestHandler<GetAllDrinksQuery, DrinkRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllDrinksHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<DrinkRecord[]> Handle(GetAllDrinksQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<DrinkRecord>(@"
                        select * from drinks"))
                .ToArray();
        }
    }
}
