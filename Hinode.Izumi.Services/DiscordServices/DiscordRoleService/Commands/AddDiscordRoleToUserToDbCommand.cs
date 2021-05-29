using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Commands
{
    public record AddDiscordRoleToUserToDbCommand(long UserId, long RoleId, DateTimeOffset Expiration) : IRequest;

    public class AddDiscordRoleToUserToDbHandler : IRequestHandler<AddDiscordRoleToUserToDbCommand>
    {
        private readonly IConnectionManager _con;

        public AddDiscordRoleToUserToDbHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddDiscordRoleToUserToDbCommand request, CancellationToken cancellationToken)
        {
            var (userId, roleId, expiration) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_roles as ur (user_id, role_id, expiration)
                    values (@userId, @roleId, @expiration)
                    on conflict (user_id, role_id) do update
                        set expiration = ur.expiration + interval '30 days',
                            updated_at = now()",
                    new {userId, roleId, expiration});

            return new Unit();
        }
    }
}
