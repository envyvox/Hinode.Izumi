using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.BuildingJob
{
    public interface IBuildingJob
    {
        /// <summary>
        /// Завершает строительство.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="projectId">Id чертеда.</param>
        Task CompleteBuilding(long userId, long projectId);
    }
}
