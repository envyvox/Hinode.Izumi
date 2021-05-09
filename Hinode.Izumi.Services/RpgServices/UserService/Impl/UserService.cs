using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.UserService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.UserService.Impl
{
    [InjectableService]
    public class UserService : IUserService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public UserService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<UserWithRowNumber[]> GetTopUsers() =>
            (await _con.GetConnection()
                .QueryAsync<UserWithRowNumber>(@"
                    select * from (
                        select *,
                               row_number() over (order by points desc, created_at desc) as RowNumber
                        from users) tmp
                    where RowNumber <= 10"))
            .ToArray();

        public async Task<UserModel> GetUser(long userId)
        {
            // получаем пользователя из базы
            var user = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserModel>(@"
                    select * from users
                    where id = @userId",
                    new {userId});

            // если пользователя нет - выводим ошибку
            if (user == null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserWithId.Parse()));

            // возвращаем пользователя
            return user;
        }

        public async Task<UserModel> GetUser(string namePattern)
        {
            // получаем пользователя из базы
            var user = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserModel>(@"
                    select * from users
                    where name ilike '%'||@namePattern||'%'",
                    new {namePattern});

            // если пользователя нет - выводим ошибку
            if (user == null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserWithName.Parse()));

            // возвращаем пользователя
            return user;
        }

        public async Task<UserWithRowNumber> GetUserWithRowNumber(long userId)
        {
            // получаем пользователя из базы
            var user = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserWithRowNumber>(@"
                    select * from (
                        select *,
                               row_number() over (order by points desc, created_at desc) as RowNumber
                        from users) tmp
                    where tmp.id = @userId",
                    new {userId});

            // если пользователя нет - выводим ошибку
            if (user == null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserWithId.Parse()));

            // возвращаем пользователя
            return user;
        }

        public async Task<UserWithRowNumber> GetUserWithRowNumber(string namePattern)
        {
            // получаем пользователя из базы
            var user = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserWithRowNumber>(@"
                    select * from (
                        select *,
                               row_number() over (order by points desc, created_at desc) as RowNumber
                        from users) tmp
                    where tmp.name ilike '%'||@namePattern||'%'",
                    new {namePattern});

            // если пользователя нет - выводим ошибку
            if (user == null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserWithName.Parse()));

            // возвращаем пользователя
            return user;
        }

        public async Task<Dictionary<Title, UserTitleModel>> GetUserTitle(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserTitleModel>(@"
                    select * from user_titles
                    where user_id = @userId",
                    new {userId}))
            .ToDictionary(x => x.Title);

        public async Task<bool> CheckUser(long userId)
        {
            // проверяем ответ в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.UserWithIdCheckKey, userId), out bool check))
                return check;

            // проверяем пользователя в базе
            check = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from users
                    where id = @userId",
                    new {userId});

            // добавляем ответ в кэш
            _cache.Set(string.Format(CacheExtensions.UserWithIdCheckKey, userId), check,
                CacheExtensions.DefaultCacheOptions);

            // возвращаем ответ
            return check;
        }

        public async Task<bool> CheckUser(string name)
        {
            // проверяем ответ в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.UserWithNameCheckKey, name), out bool check))
                return check;

            // проверяем пользователя в базе
            check = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from users
                    where name = @name",
                    new {name});

            // добавляем ответ в кэш
            _cache.Set(string.Format(CacheExtensions.UserWithNameCheckKey, name), check,
                CacheExtensions.DefaultCacheOptions);

            // возвращаем ответ
            return check;
        }

        public async Task AddUser(long userId, string name)
        {
            // добавляем пользователя в базу
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserModel>(@"
                    insert into users(id, name)
                    values (@userId, @name)
                    returning *",
                    new {userId, name});

            // добавляем пользователя в кэш
            _cache.Set(string.Format(CacheExtensions.UserWithIdCheckKey, userId), true,
                CacheExtensions.DefaultCacheOptions);
            _cache.Set(string.Format(CacheExtensions.UserWithNameCheckKey, name), true,
                CacheExtensions.DefaultCacheOptions);
        }

        public async Task RemoveUser(long userId)
        {
            // удаляем пользователя из базы
            var user = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserModel>(@"
                    delete from users
                    where id = @userId
                    returning name",
                    new {userId});

            // удаляем пользователя из кэша
            _cache.Remove(string.Format(CacheExtensions.UserWithIdCheckKey, userId));
            _cache.Remove(string.Format(CacheExtensions.UserWithNameCheckKey, user.Name));
            _cache.Set(string.Format(CacheExtensions.UserWithIdCheckKey, userId), false,
                CacheExtensions.DefaultCacheOptions);
            _cache.Set(string.Format(CacheExtensions.UserWithNameCheckKey, user.Name), false,
                CacheExtensions.DefaultCacheOptions);
        }

        public async Task AddTitleToUser(long userId, Title title) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_titles(user_id, title)
                    values (@userId, @title)
                    on conflict (user_id, title) do nothing",
                    new {userId, title});

        public async Task AddTitleToUser(long[] usersId, Title title) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_titles(user_id, title)
                    values (unnest(array[@usersId]), @title)
                    on conflict (user_id, title) do nothing",
                    new {usersId, title});

        public async Task UpdateUserName(long userId, string name)
        {
            // удаляем пользователя из кэша
            _cache.Remove(string.Format(CacheExtensions.UserWithIdCheckKey, userId));
            // обновляем базу
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set name = @name,
                        updated_at = now()
                    where id = @userId",
                    new {userId, name});
        }

        public async Task UpdateUserAbout(long userId, string about)
        {
            // удаляем пользователя из кэша
            _cache.Remove(string.Format(CacheExtensions.UserWithIdCheckKey, userId));
            // обновляем базу
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set about = @about,
                        updated_at = now()
                    where id = @userId",
                    new {userId, about});
        }

        public async Task UpdateUserGender(long userId, Gender gender)
        {
            // удаляем пользователя из кэша
            _cache.Remove(string.Format(CacheExtensions.UserWithIdCheckKey, userId));
            // обновляем базу
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set gender = @gender,
                        updated_at = now()
                    where id = @userId",
                    new {userId, gender});
        }

        public async Task UpdateUserTitle(long userId, Title title)
        {
            // удаляем пользователя из кэша
            _cache.Remove(string.Format(CacheExtensions.UserWithIdCheckKey, userId));
            // обновляем базу
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set title = @title,
                        updated_at = now()
                    where id = @userId",
                    new {userId, title});
        }

        public async Task AddEnergyToUser(long userId, long amount) =>
            // добавляем энергию, убеждаясь что новое значение не будет выше 100
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set energy = (
                        case when energy + @amount <= 100 then energy + @amount
                             else 100
                        end),
                        updated_at = now()
                    where id = @userId",
                    new {userId, amount});

        public async Task AddEnergyToUser(long[] usersId, long amount) =>
            // добавляем энергию, убеждаясь что новое значение не будет выше 100
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set energy = (
                        case when energy + @amount <= 100 then energy + @amount
                             else 100
                        end),
                        updated_at = now()
                    where id = any(array[@usersId])",
                    new {usersId, amount});

        public async Task RemoveEnergyFromUser(long userId, long amount)
        {
            // удаляем пользователя из кэша
            _cache.Remove(string.Format(CacheExtensions.UserWithIdCheckKey, userId));
            // отнимаем энергию, убеждаясь что новое значение не будет ниже 0
            // добавляем очки приключений за каждую потраченную единицу энергии
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set energy = (
                        case when energy - @amount >= 0 then energy - @amount
                             else 0
                        end),
                        points = (
                            case when energy - @amount > 0 then points + @amount
                            else points + users.energy
                        end),
                        updated_at = now()
                    where id = @userId",
                    new {userId, amount});
        }
    }
}
