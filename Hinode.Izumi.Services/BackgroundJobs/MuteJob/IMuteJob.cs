using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.MuteJob
{
    public interface IMuteJob
    {
        /// <summary>
        /// Снимает блокировку чата с пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        Task Unmute(long userId);
    }
}
