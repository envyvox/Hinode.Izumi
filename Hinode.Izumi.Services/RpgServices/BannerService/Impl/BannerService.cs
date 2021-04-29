using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.BannerService.Models;

namespace Hinode.Izumi.Services.RpgServices.BannerService.Impl
{
    [InjectableService]
    public class BannerService : IBannerService
    {
        private readonly IConnectionManager _con;

        public BannerService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<BannerInUser> GetUserBanner(long userId, long bannerId)
        {
            // получаем баннер из базы
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<BannerInUser>(@"
                    select * from user_banners as ub
                        inner join banners b
                            on b.id = ub.banner_id
                    where ub.user_id = @userId
                      and ub.banner_id = @bannerId",
                    new {userId, bannerId});

            // если у пользователя нет такого баннера - выводим ошибку
            if (res == null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserBanner.Parse()));

            // возвращаем баннер
            return res;
        }

        public async Task<IEnumerable<BannerInUser>> GetUserBanner(long userId) =>
            await _con.GetConnection()
                .QueryAsync<BannerInUser>(@"
                    select * from user_banners as ub
                        inner join banners b
                            on b.id = ub.banner_id
                    where ub.user_id = @userId
                    order by b.id",
                    new {userId});

        public async Task<BannerInUser> GetUserActiveBanner(long userId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<BannerInUser>(@"
                    select * from user_banners as ub
                        inner join banners b
                            on b.id = ub.banner_id
                    where ub.user_id = @userId
                      and active = true",
                    new {userId});

        public async Task<BannerModel> GetDynamicShopBanner(long bannerId)
        {
            // получаем баннер из динамического магазина
            var res = await _con
                .GetConnection()
                .QueryFirstOrDefaultAsync<BannerModel>(@"
                    select b.* from dynamic_shop_banners as dsb
                        inner join banners b
                            on b.id = dsb.banner_id
                    where dsb.banner_id = @bannerId",
                    new {bannerId});

            // если в динамическом магазине нет такого баннера - выводим ошибку
            if (res == null)
                await Task.FromException(new Exception(IzumiNullableMessage.DynamicShopBanner.Parse()));

            // возвращаем баннер
            return res;
        }

        public async Task<IEnumerable<BannerModel>> GetDynamicShopBanner() =>
            await _con.GetConnection()
                .QueryAsync<BannerModel>(@"
                    select * from dynamic_shop_banners as dsb
                        inner join banners b
                            on b.id = dsb.banner_id");

        public async Task<bool> CheckUserHasBanner(long userId, long bannerId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_banners
                    where user_id = @userId
                      and banner_id = @bannerId");

        public async Task AddBannerToUser(long userId, long bannerId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_banners(user_id, banner_id, active)
                    values (@userId, @bannerId, false)
                    on conflict (user_id, banner_id) do nothing",
                    new {userId, bannerId});

        public async Task ToggleBannerInUser(long userId, long bannerId, bool newStatus) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_banners
                    set active = @newStatus
                    where user_id = @userId
                      and banner_id = @bannerId",
                    new {userId, bannerId, newStatus});
    }
}
