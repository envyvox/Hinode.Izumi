using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.RpgServices.MasteryService.Models;

namespace Hinode.Izumi.Services.RpgServices.MasteryService
{
    public interface IMasteryService
    {
        /// <summary>
        /// Возвращает мастерство пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="mastery">Мастерство.</param>
        /// <returns>Мастерство пользователя.</returns>
        Task<UserMasteryModel> GetUserMastery(long userId, Mastery mastery);

        /// <summary>
        /// Возвращает библиотеку мастерства пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Библиотека мастерства пользователя.</returns>
        Task<Dictionary<Mastery, UserMasteryModel>> GetUserMastery(long userId);

        /// <summary>
        /// Добавляет мастерство пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="mastery">Мастерство.</param>
        /// <param name="amount">Количество мастерства.</param>
        Task AddMasteryToUser(long userId, Mastery mastery, double amount);

        /// <summary>
        /// Отнимает мастерство у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="mastery">Мастерство.</param>
        /// <param name="amount">Количество мастерства.</param>
        Task RemoveMasteryFromUser(long userId, Mastery mastery, double amount);
    }
}
