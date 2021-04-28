using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.MasteryService.Models;

namespace Hinode.Izumi.Services.RpgServices.MasteryService.Impl
{
    [InjectableService]
    public class MasteryService : IMasteryService
    {
        private readonly IConnectionManager _con;

        public MasteryService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserMasteryModel> GetUserMastery(long userId, Mastery mastery) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserMasteryModel>(@"
                    select * from user_masteries
                    where user_id = @userId
                      and mastery = @mastery",
                    new {userId, mastery})
            ?? new UserMasteryModel {UserId = userId, Mastery = mastery, Amount = 0};

        public async Task<Dictionary<Mastery, UserMasteryModel>> GetUserMastery(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserMasteryModel>(@"
                    select * from user_masteries
                    where user_id = @userId",
                    new {userId}))
            .ToDictionary(x => x.Mastery);

        public async Task AddMasteryToUser(long userId, Mastery mastery, double amount) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_masteries as um (user_id, mastery, amount)
                    values (@userId, @mastery, @amount)
                    on conflict (user_id, mastery) do update
                        set amount = um.amount + @amount,
                            updated_at = now()",
                    new {userId, mastery, amount});

        public async Task RemoveMasteryFromUser(long userId, Mastery mastery, double amount) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_masteries
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and mastery = @mastery",
                    new {userId, mastery, amount});
    }
}
