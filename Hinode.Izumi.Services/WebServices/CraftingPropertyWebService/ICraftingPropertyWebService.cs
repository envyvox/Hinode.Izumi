using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.CraftingPropertyWebService.Models;

namespace Hinode.Izumi.Services.WebServices.CraftingPropertyWebService
{
    public interface ICraftingPropertyWebService
    {
        Task<IEnumerable<CraftingPropertyWebModel>> GetAllCraftingProperties();
        Task<CraftingPropertyWebModel> Get(long id);
        Task<CraftingPropertyWebModel> Update(CraftingPropertyWebModel model);
        Task Upload();
    }
}
