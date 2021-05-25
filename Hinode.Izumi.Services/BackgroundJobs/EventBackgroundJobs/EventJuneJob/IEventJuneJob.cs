using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.EventBackgroundJobs.EventJuneJob
{
    public interface IEventJuneJob
    {
        Task Start();

        Task End();

        Task RemoveEventGatheringFromUsers();
    }
}
