using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Queries
{
    public record GetFamilyCurrenciesQuery(long FamilyId) : IRequest<Dictionary<Currency, FamilyCurrencyRecord>>;

    public class GetFamilyCurrenciesHandler
        : IRequestHandler<GetFamilyCurrenciesQuery, Dictionary<Currency, FamilyCurrencyRecord>>
    {
        private readonly IConnectionManager _con;

        public GetFamilyCurrenciesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Dictionary<Currency, FamilyCurrencyRecord>> Handle(GetFamilyCurrenciesQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<FamilyCurrencyRecord>(@"
                        select * from family_currencies
                        where family_id = @familyId",
                        new {familyId = request.FamilyId}))
                .ToDictionary(x => x.Currency);
        }
    }
}
