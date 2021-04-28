using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.BackgroundJobs.TransitJob
{
    public interface ITransitJob
    {
        /// <summary>
        /// Завершает перемещение пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="destination">Локация прибытия.</param>
        Task CompleteTransit(long userId, Location destination);
    }
}
