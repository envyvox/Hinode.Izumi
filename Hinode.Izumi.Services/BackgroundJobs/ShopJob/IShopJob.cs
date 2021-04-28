using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.ShopJob
{
    public interface IShopJob
    {
        /// <summary>
        /// Обновляет товары в магазине баннеров.
        /// </summary>
        Task UpdateBannersInDynamicShop();
    }
}
