using System;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.RpgServices.LocationService.Models;
using DiscordRole = Hinode.Izumi.Data.Enums.DiscordEnums.DiscordRole;

namespace Hinode.Izumi.Services.RpgServices.LocationService
{
    public interface ILocationService
    {
        /// <summary>
        /// Возвращает информацию об отправлении.
        /// </summary>
        /// <param name="departure">Локация отправления.</param>
        /// <param name="destination">Локация назначения.</param>
        /// <returns>Информация об отправлении.</returns>
        Task<TransitModel> GetTransit(Location departure, Location destination);

        /// <summary>
        /// Возвращает массив доступных отправлениях из локации.
        /// </summary>
        /// <param name="departure">Локация отправления.</param>
        /// <returns>Массив доступных отправлениях из локации.</returns>
        Task<TransitModel[]> GetTransit(Location departure);

        /// <summary>
        /// Возвращает информацию о перемещении пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Информация о перемещении пользователя.</returns>
        Task<MovementModel> GetUserMovement(long userId);

        /// <summary>
        /// Возвращает текущую локацию пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Текущая локация пользователя.</returns>
        Task<Location> GetUserLocation(long userId);

        /// <summary>
        /// Начинает перемещение пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="departure">Локация отправления.</param>
        /// <param name="destination">Локация назначения.</param>
        /// <param name="time">Длительность перемещения.</param>
        Task StartUserTransit(long userId, Location departure, Location destination, long time);

        /// <summary>
        /// Обновляет текущую локацию пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="location">Локация.</param>
        Task UpdateUserLocation(long userId, Location location);

        /// <summary>
        /// Добавляет информацию о перемещении пользователя..
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="departure">Локация отправления.</param>
        /// <param name="destination">Локация назначения.</param>
        /// <param name="arrival">Дата прибытия.</param>
        Task AddUserMovement(long userId, Location departure, Location destination, DateTimeOffset arrival);

        /// <summary>
        /// Удаляет информацию о перемещении пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        Task RemoveUserMovement(long userId);

        /// <summary>
        /// Возвращает роль на сервере соответствующую указанной локации.
        /// </summary>
        /// <param name="location">Локация.</param>
        DiscordRole GetLocationRole(Location location);
    }
}
