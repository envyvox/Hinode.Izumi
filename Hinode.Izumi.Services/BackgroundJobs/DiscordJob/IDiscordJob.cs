using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.DiscordJob
{
    public interface IDiscordJob
    {
        /// <summary>
        /// Удаляет сообщение.
        /// </summary>
        /// <param name="channelId">Id канала.</param>
        /// <param name="messageId">Id сообщения.</param>
        Task DeleteMessage(long channelId, long messageId);

        /// <summary>
        /// Снимает истекшие роли с пользователей.
        /// </summary>
        Task RemoveExpiredRoleFromUsers();
    }
}
