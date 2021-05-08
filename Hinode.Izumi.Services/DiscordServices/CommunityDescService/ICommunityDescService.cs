using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Models;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Models;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService
{
    public interface ICommunityDescService
    {
        /// <summary>
        /// Возвращает id текстовых каналов, которые входят в категорию доски сообщества.
        /// </summary>
        /// <param name="channels">Библиотека каналов сервера.</param>
        /// <returns>Массив id каналов.</returns>
        List<ulong> CommunityDescChannels(Dictionary<DiscordChannel, DiscordChannelModel> channels);

        /// <summary>
        /// Возвращает сохраненное в базе сообщение пользователя в доске сообщества.
        /// </summary>
        /// <param name="channelId">Id канала.</param>
        /// <param name="messageId">Id сообщения.</param>
        /// <returns>Сообщение пользователя в доске сообщества.</returns>
        Task<ContentMessageModel> GetContentMessage(long channelId, long messageId);

        /// <summary>
        /// Возвращает сохраненное в базе сообщение пользователя в доске сообщества.
        /// </summary>
        /// <param name="id">Id сообщения в базе.</param>
        /// <returns>Сообщение пользователя в доске сообщества.</returns>
        Task<ContentMessageModel> GetContentMessage(long id);

        /// <summary>
        /// Возвращает поставленную пользователем реакцию на указанное сообщение.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="messageId">Id сообщения в базе.</param>
        /// <returns>Поставленная пользователем реакция на сообщение.</returns>
        Task<Dictionary<Vote, ContentVoteModel>> GetUserVotesOnMessage(long userId, long messageId);

        /// <summary>
        /// Добавляет сообщение пользователя в доске сообщества в базу.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="channelId">Id канала.</param>
        /// <param name="messageId">Id сообщения.</param>
        Task AddContentMessage(long userId, long channelId, long messageId);

        /// <summary>
        /// Удаляет сообщение пользователя в доске сообщества из базы.
        /// </summary>
        /// <param name="channelId">Id канала.</param>
        /// <param name="messageId">Id сообщения.</param>
        Task RemoveContentMessage(long channelId, long messageId);

        /// <summary>
        /// Добавляет реакцию пользователя на сообщение в базу.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="messageId">Id сообщения в базе.</param>
        /// <param name="vote">Реакция.</param>
        Task AddUserVote(long userId, long messageId, Vote vote);

        /// <summary>
        /// Активирует реакцию пользователя на сообщение в базе.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="messageId">Id сообщения в базе.</param>
        /// <param name="vote">Реакция.</param>
        Task ActivateUserVote(long userId, long messageId, Vote vote);

        /// <summary>
        /// Деактивирует реакцию пользователя на сообщение из базы.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="messageId">Id сообщения в базе.</param>
        /// <param name="vote">Реакция.</param>
        Task DeactivateUserVote(long userId, long messageId, Vote vote);
    }
}
