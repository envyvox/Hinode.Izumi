using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Services.RpgServices.ReputationService.Models;

namespace Hinode.Izumi.Services.RpgServices.ReputationService
{
    public interface IReputationService
    {
        /// <summary>
        /// Возвращает репутацию пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="reputation">Репутация.</param>
        /// <returns>Репутация пользователя.</returns>
        Task<UserReputationModel> GetUserReputation(long userId, Reputation reputation);

        /// <summary>
        /// Возвращает библиотеку репутации пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Библиотека репутации пользователя.</returns>
        Task<Dictionary<Reputation, UserReputationModel>> GetUserReputation(long userId);

        /// <summary>
        /// Добавляет репутацию пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="reputation">Репутация.</param>
        /// <param name="amount">Количество репутации.</param>
        Task AddReputationToUser(long userId, Reputation reputation, long amount);

        /// <summary>
        /// Добавляет репутацию массиву пользователей.
        /// </summary>
        /// <param name="usersId">Массив id пользователей..</param>
        /// <param name="reputation">Репутация.</param>
        /// <param name="amount">Количество репутации.</param>
        Task AddReputationToUser(long[] usersId, Reputation reputation, long amount);

        /// <summary>
        /// Возвращает репутацию указанной локации.
        /// </summary>
        /// <param name="location">Локация.</param>
        /// <returns>Репутация.</returns>
        Reputation GetReputationByLocation(Location location);

        double UserMaxMastery(Dictionary<Reputation, UserReputationModel> userReputations);
    }
}
