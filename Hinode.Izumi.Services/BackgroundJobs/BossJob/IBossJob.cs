using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.ReputationEnums;

namespace Hinode.Izumi.Services.BackgroundJobs.BossJob
{
    public interface IBossJob
    {
        /// <summary>
        /// Отправляет уведомление о вторжении ежедневного босса.
        /// </summary>
        Task Anons();

        /// <summary>
        /// Создает ежедневного босса.
        /// </summary>
        Task Spawn(Location location);

        /// <summary>
        /// Проверка на убийство ежедневного босса.
        /// </summary>
        Task Kill(long channelId, long messageId, Reputation reputation, Npc npc, Image bossImage, Box box);

        /// <summary>
        /// Сбрасывает дебафф от вторждения ежедневного босса.
        /// </summary>
        Task ResetDebuff();
    }
}
