using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Models;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService
{
    public interface IDiscordGuildService
    {
        /// <summary>
        /// Возвращает пользователя дискорда.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Пользователь дискорда.</returns>
        Task<SocketUser> GetSocketUser(long userId);

        /// <summary>
        /// Возвращает текстовый канал дискорда.
        /// </summary>
        /// <param name="channelId">Id канала.</param>
        /// <returns>Текстовый канал дискорда.</returns>
        Task<SocketTextChannel> GetSocketTextChannel(long channelId);

        /// <summary>
        /// Возвращает сообщение дискорда.
        /// </summary>
        /// <param name="channelId">Id канала.</param>
        /// <param name="messageId">Id сообщения.</param>
        /// <returns></returns>
        Task<IUserMessage> GetIUserMessage(long channelId, long messageId);

        /// <summary>
        /// Возвращает библиотеку каналов дискорда из базы. Если их нет в базе - создает и добавляет.
        /// </summary>
        /// <returns>Библиотека каналов дискорда.</returns>
        Task<Dictionary<DiscordChannel, DiscordChannelModel>> GetChannels();

        /// <summary>
        /// Возвращает библиотеку ролей дискорда из базы. Если их нет в базе - создает и добавляет.
        /// </summary>
        /// <returns>Библиотека ролей дискорда.</returns>
        Task<Dictionary<DiscordRole, DiscordRoleModel>> GetRoles();

        /// <summary>
        /// Проверяет есть ли у пользователя указанная роль.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="role">Роль дискорда.</param>
        /// <returns>True если есть, false если нет.</returns>
        Task<bool> CheckRoleInUser(long userId, DiscordRole role);

        /// <summary>
        /// Изменяет имя на сервере пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="username">Новое имя.</param>
        Task Rename(long userId, string username);

        /// <summary>
        /// Добавляет или снимает роль у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="role">Роль дискорда.</param>
        /// <param name="adding">True если нужно добавить роль, false если снять.</param>
        Task ToggleRoleInUser(long userId, DiscordRole role, bool adding);

        /// <summary>
        /// Двигает пользователя в указанный голосовой канал.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="channel">Голосовой канал дискорда.</param>
        Task MoveUserInChannel(long userId, RestVoiceChannel channel);
    }
}
