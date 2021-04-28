using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.RpgServices.ContractService.Models;

namespace Hinode.Izumi.Services.RpgServices.ContractService
{
    public interface IContractService
    {
        /// <summary>
        /// Возвращает рабочий контракт.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Рабочий контракт.</returns>
        Task<ContractModel> GetContract(long id);

        /// <summary>
        /// Возвращает массив рабочих контрактов доступных в указанной локации.
        /// </summary>
        /// <param name="location">Локация.</param>
        /// <returns>Массив рабочих контрактов.</returns>
        Task<ContractModel[]> GetContract(Location location);

        /// <summary>
        /// Возвращает текущий рабочий контракт пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Рабочий контракт пользователя.</returns>
        Task<ContractModel> GetUserContract(long userId);

        /// <summary>
        /// Добавляет рабочий контракт пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="contractId">Id контракта.</param>
        Task AddContractToUser(long userId, long contractId);

        /// <summary>
        /// Забирает рабочий контракт у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        Task RemoveContractFromUser(long userId);
    }
}
