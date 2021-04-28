using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;

namespace Hinode.Izumi.Services.BackgroundJobs.ShopJob
{
    [InjectableService]
    public class ShopJob : IShopJob
    {
        private readonly IConnectionManager _con;

        public ShopJob(IConnectionManager con)
        {
            _con = con;
        }

        public async Task UpdateBannersInDynamicShop() =>
            await _con.GetConnection()
                .ExecuteAsync(
                    // удаляем все баннеры из динамического магазина
                    "delete from dynamic_shop_banners;"
                    // добавляем 5 случайных баннеров
                    // за исключением баннера по-умолчанию (id: 1) и персональных баннеров
                    + @"
                    insert into dynamic_shop_banners(banner_id)
                    select id from banners
                    where id != 1
                      and rarity != @personalBannerRarity
                    order by random()
                    limit 5",
                    new {personalBannerRarity = BannerRarity.Personal});
    }
}
