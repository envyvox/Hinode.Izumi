using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Commands
{
    public record DeleteDiscordUserRoleFromDbCommand(long UserId, long RoleId) : IRequest;

    public class DeleteDiscordUserRoleFromDbHandler : IRequestHandler<DeleteDiscordUserRoleFromDbCommand>
    {
        private readonly IConnectionManager _con;

        public DeleteDiscordUserRoleFromDbHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(DeleteDiscordUserRoleFromDbCommand request, CancellationToken cancellationToken)
        {
            var (userId, roleId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_roles
                    where user_id = @userId
                      and role_id = @roleId",
                    new {userId, roleId});

            return new Unit();
        }
    }
}
