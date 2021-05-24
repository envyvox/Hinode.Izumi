using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ContractService.Commands
{
    public record AddContractToUserCommand(long UserId, long ContractId) : IRequest;

    public class AddContractToUserHandler : IRequestHandler<AddContractToUserCommand>
    {
        private readonly IConnectionManager _con;

        public AddContractToUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddContractToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, contractId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_contracts(user_id, contract_id)
                    values (@userId, @contractId)
                    on conflict (user_id) do nothing",
                    new {userId, contractId});

            return new Unit();
        }
    }
}
