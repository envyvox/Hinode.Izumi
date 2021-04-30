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
        /// Завершает событие.
        /// </summary>
        Task End();

        /// <summary>
        /// Оповещает о появлении мангала.
        /// </summary>
        Task PicnicAnons();

        /// <summary>
        /// Создает мангал.
        /// </summary>
        Task PicnicSpawn();

        /// <summary>
        /// Убирает мангал и выдает награды.
        /// </summary>
        /// <param name="channelId">Id канала.</param>
        /// <param name="messageId">Id сообщения.</param>
        Task PicnicEnd(long channelId, long messageId);

        /// <summary>
        /// Оповещает о вторжении босса события.
        /// </summary>
        Task BossAnons();

        /// <summary>
        /// Создает босса события.
        /// </summary>
        Task BossSpawn();

        /// <summary>
        /// Убивает босса события.
        /// </summary>
        Task BossKill(long channelId, long messageId);
    }
}
