using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CropService.Queries;
using Hinode.Izumi.Services.GameServices.CropService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CropService.Handlers
{
    public class GetRandomCropHandler : IRequestHandler<GetRandomCropQuery, CropRecord>
    {
        private readonly IConnectionManager _con;

        public GetRandomCropHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<CropRecord> Handle(GetRandomCropQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CropRecord>(@"
                    select * from crops
                    order by random()
                    limit 1");
        }
    }
}
