using System;
using System.Threading.Tasks;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Models;

namespace Hinode.Izumi.Services.DiscordServices.DiscordRoleService
{
    public interface IDiscordRoleService
    {
        /// <summary>
        /// Возвращает массив истекших ролей.
        /// </summary>
        /// <returns>Массив ролей.</returns>
        Task<UserRoleModel[]> GetExpiredUserRoles();

        /// <summary>
        /// Добавляет полученную пользователем роль дискорда в базу.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="roleId">Id роли.</param>
        /// <param name="expiration">Срок действия.</param>
        Task AddRoleToUser(long userId, long roleId, DateTimeOffset expiration);
    }
}
