using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FoodService.Handlers
{
    public class GetAllSellableFoodHandler : IRequestHandler<GetAllSellableFoodQuery, FoodRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllSellableFoodHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FoodRecord[]> Handle(GetAllSellableFoodQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<FoodRecord>(@"
                        select * from foods
                        where recipe_sellable = true
                        order by id"))
                .ToArray();
        }
    }
}
