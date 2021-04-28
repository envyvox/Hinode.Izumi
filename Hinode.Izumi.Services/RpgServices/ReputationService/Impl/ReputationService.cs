using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.ReputationService.Models;

namespace Hinode.Izumi.Services.RpgServices.ReputationService.Impl
{
    [InjectableService]
    public class ReputationService : IReputationService
    {
        private readonly IConnectionManager _con;

        public ReputationService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserReputationModel> GetUserReputation(long userId, Reputation reputation) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserReputationModel>(@"
                    select * from user_reputations
                    where user_id = @userId
                      and reputation = @reputation",
                    new {userId, reputation});

        public async Task<Dictionary<Reputation, UserReputationModel>> GetUserReputation(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserReputationModel>(@"
                    select * from user_reputations
                    where user_id = @userId",
                    new {userId}))
            .ToDictionary(x => x.Reputation);

        public async Task AddReputationToUser(long userId, Reputation reputation, long amount) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_reputations as ur (user_id, reputation, amount)
                    values (@userId, @reputation, @amount)
                    on conflict (user_id, reputation) do update
                        set amount = ur.amount + @amount,
                            updated_at = now()",
                    new {userId, reputation, amount});

        public async Task AddReputationToUser(long[] usersId, Reputation reputation, long amount) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_reputations as ur (user_id, reputation, amount)
                    values (unnest(array[@usersId]), @reputation, @amount)
                    on conflict (user_id, reputation) do update
                        set amount = ur.reputation + @reputation,
                            updated_at = now()",
                    new {usersId, reputation, amount});


        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        public Reputation GetReputationByLocation(Location location) => location switch
        {
            Location.Capital => Reputation.Capital,
            Location.Garden => Reputation.Garden,
            Location.Seaport => Reputation.Seaport,
            Location.Castle => Reputation.Castle,
            Location.Village => Reputation.Village,
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };
    }
}
