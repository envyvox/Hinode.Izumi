using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Models;

namespace Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Impl
{
    [InjectableService]
    public class DiscordRoleService : IDiscordRoleService
    {
        private readonly IConnectionManager _con;

        public DiscordRoleService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserRoleModel[]> GetExpiredUserRoles() =>
            (await _con.GetConnection()
                .QueryAsync<UserRoleModel>(@"
                    select * from user_roles
                    where expiration < now()"))
            .ToArray();

        public async Task AddRoleToUser(long userId, long roleId, DateTimeOffset expiration) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_roles as ur (user_id, role_id, expiration)
                    values (@userId, @roleId, @expiration)
                    on conflict (user_id, role_id) do update
                        set expiration = ur.expiration + interval '30 days',
                            updated_at = now()",
                    new {userId, roleId, expiration});
    }
}
