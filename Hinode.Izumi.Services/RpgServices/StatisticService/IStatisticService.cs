using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.RpgServices.StatisticService.Models;

namespace Hinode.Izumi.Services.RpgServices.StatisticService
{
    public interface IStatisticService
    {
        /// <summary>
        /// Возвращает статистику пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="statistic">Статистика.</param>
        /// <returns>Статистика пользователя.</returns>
        Task<UserStatisticModel> GetUserStatistic(long userId, Statistic statistic);

        /// <summary>
        /// Добавляет статистику пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="statistic">Статистика.</param>
        /// <param name="amount">Количество (по-умолчанию 1).</param>
        Task AddStatisticToUser(long userId, Statistic statistic, long amount = 1);

        /// <summary>
        /// Добавляет статистику массиву пользователей.
        /// </summary>
        /// <param name="usersId">Массив Id пользователей.</param>
        /// <param name="statistic">Статистика.</param>
        /// <param name="amount">Количество (по-умолчанию 1).</param>
        Task AddStatisticToUser(long[] usersId, Statistic statistic, long amount = 1);
    }
}
