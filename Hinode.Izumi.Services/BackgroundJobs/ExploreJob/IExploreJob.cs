using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.ExploreJob
{
    public interface IExploreJob
    {
        /// <summary>
        /// Завершает исследование в саду.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="userGatheringMastery">Мастерство сбора пользователя.</param>
        Task CompleteExploreGarden(long userId, long userGatheringMastery);

        /// <summary>
        /// Завершает исследование в замке.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="userGatheringMastery">Мастерство сбора пользователя.</param>
        Task CompleteExploreCastle(long userId, long userGatheringMastery);

        /// <summary>
        /// Завершает рыбалку.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="userFishingMastery">Мастерсто рыбалки пользователя.</param>
        Task CompleteFishing(long userId, long userFishingMastery);
    }
}
