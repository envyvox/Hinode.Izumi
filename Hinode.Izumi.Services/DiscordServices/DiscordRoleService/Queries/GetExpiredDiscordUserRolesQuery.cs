using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Records;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Queries
{
    public record GetExpiredDiscordUserRolesQuery : IRequest<DiscordUserRoleRecord[]>;

    public class GetExpiredDiscordUserRolesHandler
        : IRequestHandler<GetExpiredDiscordUserRolesQuery, DiscordUserRoleRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetExpiredDiscordUserRolesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<DiscordUserRoleRecord[]> Handle(GetExpiredDiscordUserRolesQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<DiscordUserRoleRecord>(@"
                        select * from user_roles
                        where expiration < now()"))
                .ToArray();
        }
    }
}
