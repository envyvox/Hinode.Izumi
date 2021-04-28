using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.StatisticService.Models;

namespace Hinode.Izumi.Services.RpgServices.StatisticService.Impl
{
    [InjectableService]
    public class StatisticService : IStatisticService
    {
        private readonly IConnectionManager _con;

        public StatisticService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserStatisticModel> GetUserStatistic(long userId, Statistic statistic) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserStatisticModel>(@"
                    select * from user_statistics
                    where user_id = @userId
                      and statistic = @statistic",
                    new {userId, statistic});

        public async Task AddStatisticToUser(long userId, Statistic statistic, long amount = 1) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_statistics as us (user_id, statistic, amount)
                    values (@userId, @statistic, @amount)
                    on conflict (user_id, statistic) do update
                        set amount = us.amount + @amount,
                            updated_at = now()",
                    new {userId, statistic, amount});

        public async Task AddStatisticToUser(long[] usersId, Statistic statistic, long amount = 1) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_statistics as us (user_id, statistic, amount)
                    values (unnest(array[@usersId]), @statistic, @amount)
                    on conflict (user_id, statistic) do update
                        set amount = us.amount + @amount,
                            updated_at = now()",
                    new {usersId, statistic, amount});
    }
}
