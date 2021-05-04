using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.FieldService.Models;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.SeedService;

namespace Hinode.Izumi.Services.RpgServices.FieldService.Impl
{
    [InjectableService]
    public class FieldService : IFieldService
    {
        private readonly IConnectionManager _con;
        private readonly IPropertyService _propertyService;
        private readonly ISeedService _seedService;

        public FieldService(IConnectionManager con, IPropertyService propertyService, ISeedService seedService)
        {
            _con = con;
            _propertyService = propertyService;
            _seedService = seedService;
        }

        public async Task<UserFieldModel> GetUserField(long userId, long fieldId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserFieldModel>(@"
                    select * from user_fields
                    where user_id = @userId
                      and field_id = @fieldId",
                    new {userId, fieldId});

        public async Task<UserFieldModel[]> GetUserField(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserFieldModel>(@"
                    select * from user_fields
                    where user_id = @userId
                    order by field_id",
                    new {userId}))
            .ToArray();

        public async Task AddFieldToUser(long userId, long[] fieldsId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_fields(user_id, field_id)
                    values (@userId, unnest(array[@fieldsId]))
                    on conflict (user_id, field_id) do nothing",
                    new {userId, fieldsId});

        public async Task Plant(long userId, long fieldId, long seedId)
        {
            // получаем погоду
            var weather = (Weather) await _propertyService.GetPropertyValue(Property.WeatherToday);
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set seed_id = @seedId,
                        state = @state,
                        updated_at = now()
                    where user_id = @userId
                      and field_id = @fieldId",
                    new
                    {
                        userId, fieldId, seedId,
                        // определяем состояение клетки земли в зависимости от погоды
                        state = weather == Weather.Clear
                            ? FieldState.Planted
                            : FieldState.Watered
                    });
        }

        public async Task StartReGrowth(long userId, long fieldId)
        {
            // получаем погоду
            var weather = (Weather) await _propertyService.GetPropertyValue(Property.WeatherToday);
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set state = @state,
                        progress = 0,
                        re_growth = true,
                        updated_at = now()",
                    new
                    {
                        userId, fieldId,
                        // определяем состояение клетки земли в зависимости от погоды
                        state = weather == Weather.Clear
                            ? FieldState.Planted
                            : FieldState.Watered
                    });
        }

        public async Task UpdateState(long userId, FieldState state) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set state = @state,
                        updated_at = now()
                    where user_id = @userId
                      and state = @planted",
                    new {userId, state, planted = FieldState.Planted});

        public async Task UpdateState(FieldState state) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set state = @state,
                        updated_at = now()
                    where state != @empty
                      and state != @completed",
                    new {state, empty = FieldState.Empty, completed = FieldState.Completed});

        public async Task MoveProgress()
        {
            // двигаем прогресс роста всех клеток земли
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set progress = progress + 1,
                        updated_at = now()
                    where state = @watered",
                    new {watered = FieldState.Watered});

            // проверяем вырос ли урожай на каких-то клетках
            await CheckFieldComplete();
        }

        public async Task ResetField(long userId, long fieldId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set state = default,
                        seed_id = default,
                        progress = default,
                        re_growth = default,
                        updated_at = now()
                    where user_id = @userId
                      and field_id = @fieldId",
                    new {userId, fieldId});

        public async Task ResetField() =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set state = default,
                        seed_id = default,
                        progress = default,
                        re_growth = default,
                        updated_at = now()");

        /// <summary>
        /// Проверяет вырос ли урожай на каких=то клетках. Если вырос, то меняет состояние клетки.
        /// </summary>
        private async Task CheckFieldComplete()
        {
            // получаем клетки земли на которых посажены семена
            var fieldsWithSeed = await _con.GetConnection()
                .QueryAsync<UserFieldModel>(@"
                    select * from user_fields
                    where state != @empty
                      and state != @completed",
                    new {empty = FieldState.Empty, completed = FieldState.Completed});
            // создаем список для готовых к сбору клеток земли
            var completedFields = new List<long>();

            // проверяем каждую клетку земли на готовность к сбору
            foreach (var field in fieldsWithSeed)
            {
                // получаем семя которое посажено на этой клетке
                var seed = await _seedService.GetSeed(field.SeedId);

                // добавляем в список клетки земли которые готовы к сбору
                // ReSharper disable once ConvertIfStatementToSwitchStatement это не читабельно
                if (!field.ReGrowth && field.Progress >= seed.Growth) completedFields.Add(field.Id);
                if (field.ReGrowth && field.Progress >= seed.ReGrowth) completedFields.Add(field.Id);
            }

            // обновляем состояние клетки земли
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_fields
                    set state = @completed,
                        updated_at = now()
                    from (
                        select unnest(array[@ids]) as cId
                        ) as new
                    where id = new.cId",
                    new {completed = FieldState.Completed, ids = completedFields.ToArray()});
        }
    }
}
