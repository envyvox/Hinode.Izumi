using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FishService.Queries;
using Hinode.Izumi.Services.GameServices.FishService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FishService.Handlers
{
    public class GetAllFishHandler : IRequestHandler<GetAllFishQuery, FishRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllFishHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FishRecord[]> Handle(GetAllFishQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<FishRecord>(@"
                        select * from fishes
                        order by id"))
                .ToArray();
        }
    }
}
