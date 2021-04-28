using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.CooldownService.Models;

namespace Hinode.Izumi.Services.RpgServices.CooldownService.Impl
{
    [InjectableService]
    public class CooldownService : ICooldownService
    {
        private readonly IConnectionManager _con;

        public CooldownService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserCooldownModel> GetUserCooldown(long userId, Cooldown cooldown) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserCooldownModel>(@"
                    select * from user_cooldowns
                    where user_id = @userId
                      and cooldown = @cooldown",
                    new {userId, cooldown}) ?? new UserCooldownModel();

        public async Task<Dictionary<Cooldown, UserCooldownModel>> GetUserCooldown(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserCooldownModel>(@"
                    select * from user_cooldowns
                    where user_id = @userId",
                    new {userId}))
            .ToDictionary(x => x.Cooldown);

        public async Task AddCooldownToUser(long userId, Cooldown cooldown, DateTime expiration) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_cooldowns(user_id, cooldown, expiration)
                    values (@userId, @cooldown, @expiration)
                    on conflict (user_id, cooldown) do update
                        set expiration = @expiration,
                            updated_at = now()",
                    new {userId, cooldown, expiration});
    }
}
