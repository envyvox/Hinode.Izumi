using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CropService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CropService.Queries
{
    public record GetAllCropsQuery : IRequest<CropRecord[]>;

    public class GetAllCropsHandler : IRequestHandler<GetAllCropsQuery, CropRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllCropsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<CropRecord[]> Handle(GetAllCropsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<CropRecord>(@"
                        select c.*, s.season from crops as c
                            inner join seeds s
                                on s.id = c.seed_id"))
                .ToArray();
        }
    }
}
