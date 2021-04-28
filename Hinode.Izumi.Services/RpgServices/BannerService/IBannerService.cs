using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.BannerService.Models;

namespace Hinode.Izumi.Services.RpgServices.BannerService
{
    public interface IBannerService
    {
        /// <summary>
        /// Возвращает баннер пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="bannerId">Id баннера.</param>
        /// <returns>Баннер пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserBanner"></exception>
        Task<BannerInUser> GetUserBanner(long userId, long bannerId);

        /// <summary>
        /// Возвращает массив баннеров пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив баннеров пользователя.</returns>
        Task<IEnumerable<BannerInUser>> GetUserBanner(long userId);

        /// <summary>
        /// Возвращает активный баннер пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Активный баннер пользователя.</returns>
        Task<BannerInUser> GetUserActiveBanner(long userId);

        /// <summary>
        /// Возвращает баннер из динамического магазина.
        /// </summary>
        /// <param name="bannerId">Id баннера.</param>
        /// <returns>Баннер из динамического магазина.</returns>
        /// <exception cref="IzumiNullableMessage.DynamicShopBanner"></exception>
        Task<BannerModel> GetDynamicShopBanner(long bannerId);

        /// <summary>
        /// Возвращает массив баннеров из динамического магазина.
        /// </summary>
        /// <returns>Массив баннеров из динамического магазина.</returns>
        Task<IEnumerable<BannerModel>> GetDynamicShopBanner();

        /// <summary>
        /// Проверяет есть ли у пользователя баннер.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="bannerId">Id баннера.</param>
        /// <returns>True если есть, false если нет.</returns>
        Task<bool> CheckUserHasBanner(long userId, long bannerId);

        /// <summary>
        /// Добавляет баннер пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="bannerId">Id баннера.</param>
        Task AddBannerToUser(long userId, long bannerId);

        /// <summary>
        /// Обновляет статус баннера пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="bannerId">Id баннера.</param>
        /// <param name="newStatus">Новый статус (true если активный, false если нет).</param>
        Task ToggleBannerInUser(long userId, long bannerId, bool newStatus);
    }
}
