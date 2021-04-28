using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.BackgroundJobs.TransitJob;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.RpgServices.LocationService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;
using DiscordRole = Hinode.Izumi.Data.Enums.DiscordEnums.DiscordRole;

namespace Hinode.Izumi.Services.RpgServices.LocationService.Impl
{
    [InjectableService]
    public class LocationService : ILocationService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly IDiscordGuildService _discordGuildService;

        public LocationService(IConnectionManager con, IMemoryCache cache, TimeZoneInfo timeZoneInfo,
            IDiscordGuildService discordGuildService)
        {
            _con = con;
            _cache = cache;
            _timeZoneInfo = timeZoneInfo;
            _discordGuildService = discordGuildService;
        }

        public async Task<TransitModel> GetTransit(Location departure, Location destination)
        {
            // проверяем отправление в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.TransitKey, departure, destination),
                out TransitModel transit))
                return transit;

            // получаем отправление из базы
            transit = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<TransitModel>(@"
                    select * from transits
                    where departure = @departure
                      and destination = @destination",
                    new {departure, destination});

            // если такой маршрут есть
            if (transit != null)
            {
                // добавляем его в кэш
                _cache.Set(string.Format(CacheExtensions.TransitKey, departure, destination), transit,
                    CacheExtensions.DefaultCacheOptions);
                // и возвращаем
                return transit;
            }

            // если такого маршрута нет - выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.Transit.Parse()));
            return new TransitModel();
        }

        public async Task<TransitModel[]> GetTransit(Location departure)
        {
            // проверяем отправления в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.TransitsLocationKey, departure),
                out TransitModel[] transits))
                return transits;

            // получаем отправления из базы
            transits = (await _con.GetConnection()
                    .QueryAsync<TransitModel>(@"
                        select * from transits
                        where departure = @departure",
                        new {departure}))
                .ToArray();

            // добавляем отправления в кэш
            _cache.Set(string.Format(CacheExtensions.TransitsLocationKey, departure), transits,
                CacheExtensions.DefaultCacheOptions);

            // возвращаем отправления
            return transits;
        }

        public async Task<MovementModel> GetUserMovement(long userId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<MovementModel>(@"
                    select * from movements
                    where user_id = @userId",
                    new {userId}) ?? new MovementModel();

        public async Task<Location> GetUserLocation(long userId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<Location>(@"
                    select location from users
                    where id = @userId",
                    new {userId});

        public async Task StartUserTransit(long userId, Location departure, Location destination, long time)
        {
            // получаем текущее время
            var timeNow = TimeZoneInfo.ConvertTime(DateTime.Now, _timeZoneInfo);
            // получаем время прибытия
            var arrival = timeNow.AddMinutes(time);

            // обновляем текущую локацию пользователя
            await UpdateUserLocation(userId, Location.InTransit);
            // добавляем информацию о перемещении
            await AddUserMovement(userId, departure, departure, arrival);

            // если локация прибытия не подлокация
            if (!destination.SubLocation())
            {
                // снимаем роль текущей локации
                await _discordGuildService.ToggleRoleInUser(userId, GetLocationRole(destination), false);
                // добавляем роль перемещения
                await _discordGuildService.ToggleRoleInUser(userId, DiscordRole.LocationInTransit, true);
            }

            // добавляем джобу окончания перемещения
            BackgroundJob.Schedule<ITransitJob>(x =>
                    x.CompleteTransit(userId, destination),
                TimeSpan.FromMinutes(time));
        }

        public async Task UpdateUserLocation(long userId, Location location) =>
            // обновляем базу
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set location = @location,
                        updated_at = now()
                    where id = @userId",
                    new {userId, location});

        public async Task AddUserMovement(long userId, Location departure, Location destination, DateTime arrival) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into movements(user_id, departure, destination, arrival)
                    values (@userId, @departure, @destination, @arrival)
                    on conflict (user_id) do nothing",
                    new {userId, departure, destination, arrival});

        public async Task RemoveUserMovement(long userId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from movements
                    where user_id = @userId",
                    new {userId});

        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        public DiscordRole GetLocationRole(Location location) => location switch
        {
            Location.InTransit => DiscordRole.LocationInTransit,
            Location.Capital => DiscordRole.LocationCapital,
            Location.Garden => DiscordRole.LocationGarden,
            Location.Seaport => DiscordRole.LocationSeaport,
            Location.Castle => DiscordRole.LocationCastle,
            Location.Village => DiscordRole.LocationVillage,
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };
    }
}
