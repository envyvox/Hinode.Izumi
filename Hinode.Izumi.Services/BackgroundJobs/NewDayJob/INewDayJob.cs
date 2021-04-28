using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.NewDayJob
{
    public interface INewDayJob
    {
        Task StartNewDay();
    }
}
