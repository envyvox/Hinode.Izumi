using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.RpgServices.CooldownService.Models;

namespace Hinode.Izumi.Services.RpgServices.CooldownService
{
    public interface ICooldownService
    {
        /// <summary>
        /// Возвращает кулдаун пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="cooldown">Кулдаун.</param>
        /// <returns>Кулдаун пользователя.</returns>
        Task<UserCooldownModel> GetUserCooldown(long userId, Cooldown cooldown);

        /// <summary>
        /// Возвращает библиотеку кулдаунов пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Библиотека кулдаунов пользователя.</returns>
        Task<Dictionary<Cooldown, UserCooldownModel>> GetUserCooldown(long userId);

        /// <summary>
        /// Добавляет кулдаун пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="cooldown">Кулдаун.</param>
        /// <param name="expiration">Дата окончания кулдауна.</param>
        Task AddCooldownToUser(long userId, Cooldown cooldown, DateTime expiration);
    }
}
