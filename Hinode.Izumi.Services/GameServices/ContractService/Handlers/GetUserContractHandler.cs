using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ContractService.Queries;
using Hinode.Izumi.Services.GameServices.ContractService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ContractService.Handlers
{
    public class GetUserContractHandler : IRequestHandler<GetUserContractQuery, ContractRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserContractHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<ContractRecord> Handle(GetUserContractQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ContractRecord>(@"
                    select c.* from user_contracts as uc
                        inner join contracts c
                            on c.id = uc.contract_id
                    where uc.user_id = @userId",
                    new {userId = request.UserId});
        }
    }
}
