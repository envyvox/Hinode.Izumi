using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class GetAllFamiliesHandler : IRequestHandler<GetAllFamiliesQuery, FamilyRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllFamiliesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FamilyRecord[]> Handle(GetAllFamiliesQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<FamilyRecord>(@"
                        select * from families"))
                .ToArray();
        }
    }
}
