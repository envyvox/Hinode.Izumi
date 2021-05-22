using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ContractService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ContractService.Handlers
{
    public class RemoveContractFromUserHandler : IRequestHandler<RemoveContractFromUserCommand>
    {
        private readonly IConnectionManager _con;

        public RemoveContractFromUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(RemoveContractFromUserCommand request, CancellationToken cancellationToken)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_contracts
                    where user_id = @userId",
                    new {userId = request.UserId});

            return new Unit();
        }
    }
}
