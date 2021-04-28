using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.ContractJob
{
    public interface IContractJob
    {
        /// <summary>
        /// Завершает работу над контрактом.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="contractId">Id контракта.</param>
        Task Execute(long userId, long contractId);
    }
}
