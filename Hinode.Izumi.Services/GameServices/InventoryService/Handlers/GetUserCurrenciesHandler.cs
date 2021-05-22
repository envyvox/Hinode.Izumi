using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Handlers
{
    public class GetUserCurrenciesHandler
        : IRequestHandler<GetUserCurrenciesQuery, Dictionary<Currency, UserCurrencyRecord>>
    {
        private readonly IConnectionManager _con;

        public GetUserCurrenciesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Dictionary<Currency, UserCurrencyRecord>> Handle(GetUserCurrenciesQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserCurrencyRecord>(@"
                        select * from user_currencies
                        where user_id = @userId
                        order by currency",
                        new {userId = request.UserId}))
                .ToDictionary(x => x.Currency);
        }
    }
}
