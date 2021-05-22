using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Handlers
{
    public class RemoveEnergyFromUserHandler : IRequestHandler<RemoveEnergyFromUserCommand>
    {
        private readonly IConnectionManager _con;

        public RemoveEnergyFromUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(RemoveEnergyFromUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, amount) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set energy = (
                        case when energy - @amount >= 0 then energy - @amount
                             else 0
                        end),
                        points = (
                            case when energy - @amount > 0 then points + @amount
                            else points + users.energy
                        end),
                        updated_at = now()
                    where id = @userId",
                    new {userId, amount});

            return new Unit();
        }
    }
}
