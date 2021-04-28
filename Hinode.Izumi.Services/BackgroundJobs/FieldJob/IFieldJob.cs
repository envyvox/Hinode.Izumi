using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.FieldJob
{
    public interface IFieldJob
    {
        /// <summary>
        /// Завершает поливку участка земли.
        /// </summary>
        /// <param name="userId">Id пользователя который поливает.</param>
        /// <param name="fieldOwnerId">Id пользователя владельца участка.</param>
        Task CompleteWatering(long userId, long fieldOwnerId);
    }
}
