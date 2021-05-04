using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.PointsJob
{
    public interface IPointsJob
    {
        /// <summary>
        /// Сбрасывает очки приключений у всех пользователей.
        /// </summary>
        Task ResetAdventurePoints();
    }
}
