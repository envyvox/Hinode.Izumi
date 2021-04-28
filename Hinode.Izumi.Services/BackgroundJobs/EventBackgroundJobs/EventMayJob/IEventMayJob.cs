using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.EventBackgroundJobs.EventMayJob
{
    public interface IEventMayJob
    {
        /// <summary>
        /// Начинает событие.
        /// </summary>
        Task Start();

        /// <summary>
        /// Оповещает о появлении мангала.
        /// </summary>
        Task GrillAnons();

        /// <summary>
        /// Создает мангал.
        /// </summary>
        Task GrillSpawn();

        /// <summary>
        /// Убирает мангал и выдает награды.
        /// </summary>
        /// <param name="channelId">Id канала.</param>
        /// <param name="messageId">Id сообщения.</param>
        Task GrillEnd(long channelId, long messageId);
    }
}
