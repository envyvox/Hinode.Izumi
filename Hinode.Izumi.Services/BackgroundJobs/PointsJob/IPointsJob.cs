using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.PointsJob
{
    public interface IPointsJob
    {
        Task SendAwardsAndResetPoints();
        Task ResetAdventurePoints();
    }
}
