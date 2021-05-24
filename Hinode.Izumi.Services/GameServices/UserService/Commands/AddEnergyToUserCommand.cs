using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record AddEnergyToUserCommand(long Id, long Amount) : IRequest;

    public class AddEnergyToUserHandler : IRequestHandler<AddEnergyToUserCommand>
    {
        private readonly IConnectionManager _con;

        public AddEnergyToUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddEnergyToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, amount) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set energy = (
                        case when energy + @amount <= 100 then energy + @amount
                             else 100
                        end),
                        updated_at = now()
                    where id = @userId",
                    new {userId, amount});

            return new Unit();
        }
    }
}
