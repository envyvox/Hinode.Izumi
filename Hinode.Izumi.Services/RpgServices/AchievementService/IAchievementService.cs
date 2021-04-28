using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Services.RpgServices.AchievementService.Models;

namespace Hinode.Izumi.Services.RpgServices.AchievementService
{
    public interface IAchievementService
    {
        /// <summary>
        /// Возвращает достижение.
        /// </summary>
        /// <param name="id">Id достижения.</param>
        /// <returns>Достижение.</returns>
        Task<AchievementModel> GetAchievement(long id);

        /// <summary>
        /// Возвращает массив достижений пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category"></param>
        /// <returns>Массив достижений пользователя.</returns>
        Task<UserAchievementModel[]> GetUserAchievement(long userId, AchievementCategory category);

        /// <summary>
        /// Проверяет нужно ли выполнить достижение пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="achievement">Достижение.</param>
        Task CheckAchievement(long userId, Achievement achievement);

        /// <summary>
        /// Проверяет нужно ли выполнить достижение у массива пользователей.
        /// </summary>
        /// <param name="usersId">Массив id пользователей.</param>
        /// <param name="achievement">Достижение.</param>
        Task CheckAchievement(IEnumerable<long> usersId, Achievement achievement);

        /// <summary>
        /// Проверяет нужно ли выполнить массив достижений пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="achievements">Массив достижений.</param>
        Task CheckAchievement(long userId, IEnumerable<Achievement> achievements);

        /// <summary>
        /// Проверяет нужно ли выполнить массив достижений у массива пользователей.
        /// </summary>
        /// <param name="usersId">Массив id пользователей.</param>
        /// <param name="achievements">Массив достижений.</param>
        Task CheckAchievement(IEnumerable<long> usersId, Achievement[] achievements);
    }
}
