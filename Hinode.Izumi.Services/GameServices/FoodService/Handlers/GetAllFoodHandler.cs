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
    public class GetAllFoodHandler : IRequestHandler<GetAllFoodQuery, FoodRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllFoodHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FoodRecord[]> Handle(GetAllFoodQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<FoodRecord>(@"
                        select * from foods
                        where event = false
                        order by id"))
                .ToArray();
        }
    }
}
