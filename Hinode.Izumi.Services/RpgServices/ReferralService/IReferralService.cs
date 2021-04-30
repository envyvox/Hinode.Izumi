using System.Threading.Tasks;
using Hinode.Izumi.Services.RpgServices.UserService.Models;

namespace Hinode.Izumi.Services.RpgServices.ReferralService
{
    public interface IReferralService
    {
        /// <summary>
        /// Возвращает пользователя который является реферерром пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Пользователя.</returns>
        Task<UserModel> GetUserReferrer(long userId);

        /// <summary>
        /// Возвращает массив пользователей которых пригласил пользователь.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив пользователей.</returns>
        Task<UserModel[]> GetUserReferrals(long userId);

        /// <summary>
        /// Проверяет есть ли у пользователя реферрер.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>True есть есть, false если нет.</returns>
        Task<bool> CheckUserHasReferrer(long userId);

        /// <summary>
        /// Возвращает количество приглашенных пользователем пользователей.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Количество приглашенных пользователей.</returns>
        Task<long> GetUserReferralCount(long userId);

        /// <summary>
        /// Добавляет пользователю реферерра.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="referrerId">Id реферрера.</param>
        Task AddUserReferrer(long userId, long referrerId);
    }
}
