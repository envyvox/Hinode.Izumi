using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Records;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Queries
{
    public record GetDiscordUserRoleQuery(long UserId, long RoleId) : IRequest<DiscordUserRoleRecord>;

    public class GetDiscordUserRoleHandler : IRequestHandler<GetDiscordUserRoleQuery, DiscordUserRoleRecord>
    {
        private readonly IConnectionManager _con;

        public GetDiscordUserRoleHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<DiscordUserRoleRecord> Handle(GetDiscordUserRoleQuery request,
            CancellationToken cancellationToken)
        {
            var (userId, roleId) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<DiscordUserRoleRecord>(@"
                    select * from user_roles
                    where user_id = @userId
                      and role_id = @roleId",
                    new {userId, roleId});
        }
    }
}
