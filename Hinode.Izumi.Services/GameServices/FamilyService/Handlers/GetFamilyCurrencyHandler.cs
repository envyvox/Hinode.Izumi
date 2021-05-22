using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class GetFamilyCurrencyHandler : IRequestHandler<GetFamilyCurrencyQuery, FamilyCurrencyRecord>
    {
        private readonly IConnectionManager _con;

        public GetFamilyCurrencyHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FamilyCurrencyRecord> Handle(GetFamilyCurrencyQuery request,
            CancellationToken cancellationToken)
        {
            var (familyId, currency) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FamilyCurrencyRecord>(@"
                    select * from family_currencies
                    where family_id = @family_id
                      and currency = @currency",
                    new {familyId, currency});
        }
    }
}
