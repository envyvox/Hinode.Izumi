using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.CraftingWebService.Models;

namespace Hinode.Izumi.Services.WebServices.CraftingWebService
{
    public interface ICraftingWebService
    {
        Task<IEnumerable<CraftingWebModel>> GetAllCrafting();
        Task<CraftingWebModel> Get(long id);
        Task<CraftingWebModel> Update(CraftingWebModel model);
        Task Remove(long id);
    }
}
