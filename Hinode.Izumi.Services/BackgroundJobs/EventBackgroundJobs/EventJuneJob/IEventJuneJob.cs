using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.EventBackgroundJobs.EventJuneJob
{
    public interface IEventJuneJob
    {
        Task Start();
        Task End();
        Task RemoveEventGatheringFromUsers();
        Task SkyLanternAnons();
        Task SkyLanternBegin();
        Task SkyLanternEnd(long channelId, long messageId);
    }
}
