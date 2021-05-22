using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CooldownService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CooldownService.Handlers
{
    public class AddCooldownToUserHandler : IRequestHandler<AddCooldownToUserCommand>
    {
        private readonly IConnectionManager _con;

        public AddCooldownToUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddCooldownToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, cooldown, expiration) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_cooldowns(user_id, cooldown, expiration)
                    values (@userId, @cooldown, @expiration)
                    on conflict (user_id, cooldown) do update
                        set expiration = @expiration,
                            updated_at = now()",
                    new {userId, cooldown, expiration});

            return new Unit();
        }
    }
}
