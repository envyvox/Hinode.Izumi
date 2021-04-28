using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.ContractWebService.Models;

namespace Hinode.Izumi.Services.WebServices.ContractWebService
{
    public interface IContractWebService
    {
        Task<IEnumerable<ContractWebModel>> GetAllContracts();
        Task<ContractWebModel> Get(long id);
        Task<ContractWebModel> Update(ContractWebModel model);
        Task Remove(long id);
    }
}
