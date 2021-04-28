using System;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Services.RpgServices.EffectService.Models;
using Hinode.Izumi.Services.RpgServices.UserService.Models;

namespace Hinode.Izumi.Services.RpgServices.EffectService
{
    public interface IEffectService
    {
        /// <summary>
        /// Возвращает массив эффектов пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив эффектов пользователя.</returns>
        Task<UserEffectModel[]> GetUserEffect(long userId);

        /// <summary>
        /// Возвращает массив пользователей с указанным эффектом.
        /// </summary>
        /// <param name="effect">Эффект.</param>
        /// <returns>Массив пользователей.</returns>
        Task<UserModel[]> GetUsersWithEffect(Effect effect);

        /// <summary>
        /// Проверяет есть ли у пользователя эффект.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="effect">Эффект.</param>
        /// <returns>True если есть, false если нет.</returns>
        Task<bool> CheckUserHasEffect(long userId, Effect effect);

        /// <summary>
        /// Добавляет эффект пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория эффекта.</param>
        /// <param name="effect">Эффект.</param>
        /// <param name="expiration">Дата окончания эффекта (не обязательно).</param>
        Task AddEffectToUser(long userId, EffectCategory category, Effect effect, DateTime? expiration = null);

        /// <summary>
        /// Снимает эффект с пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="effect">Эффект.</param>
        Task RemoveEffectFromUser(long userId, Effect effect);
    }
}
