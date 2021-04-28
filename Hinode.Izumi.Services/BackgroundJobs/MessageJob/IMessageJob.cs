using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.MessageJob
{
    public interface IMessageJob
    {
        /// <summary>
        /// Удаляет сообщение.
        /// </summary>
        /// <param name="channelId">Id канала.</param>
        /// <param name="messageId">Id сообщения.</param>
        Task Delete(long channelId, long messageId);
    }
}
