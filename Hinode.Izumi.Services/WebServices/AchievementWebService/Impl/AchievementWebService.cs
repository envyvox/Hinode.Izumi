using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.AchievementWebService.Models;

namespace Hinode.Izumi.Services.WebServices.AchievementWebService.Impl
{
    [InjectableService]
    public class AchievementWebService : IAchievementWebService
    {
        private readonly IConnectionManager _con;

        public AchievementWebService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<IEnumerable<AchievementWebModel>> GetAllAchievements() =>
            await _con.GetConnection()
                .QueryAsync<AchievementWebModel>(@"
                    select * from achievements
                    order by id");

        public async Task<AchievementWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AchievementWebModel>(@"
                    select * from achievements
                    where id = @id",
                    new {id});

        public async Task<AchievementWebModel> Update(AchievementWebModel model) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AchievementWebModel>(@"
                    insert into achievements(category, type, reward, number)
                    values (@category, @type, @reward, @number)
                    on conflict (type) do update
                    set reward = @reward,
                        number = @number,
                        updated_at = now()
                    returning *",
                    new
                    {
                        category = model.Category,
                        type = model.Type,
                        reward = model.Reward,
                        number = model.Number
                    });

        public async Task Upload()
        {
            var achievementCategories = new List<long>();
            var achievements = new List<long>();

            foreach (var achievement in Enum.GetValues(typeof(Achievement)).Cast<Achievement>())
            {
                achievementCategories.Add(achievement.Category().GetHashCode());
                achievements.Add(achievement.GetHashCode());
            }

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into achievements(category, type, reward, number)
                    values (unnest(array[@achievementCategories]), unnest(array[@achievements]), 1, 0)
                    on conflict (type) do nothing",
                    new {achievementCategories, achievements});
        }
    }
}
