using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.CraftingIngredientWebService.Models;

namespace Hinode.Izumi.Services.WebServices.CraftingIngredientWebService
{
    public interface ICraftingIngredientWebService
    {
        Task<IEnumerable<CraftingIngredientWebModel>> GetAllCraftingIngredients();
        Task<IEnumerable<CraftingIngredientWebModel>> GetCraftingIngredients(long craftingId);
        Task<CraftingIngredientWebModel> Get(long id);
        Task<CraftingIngredientWebModel> Update(CraftingIngredientWebModel model);
        Task Remove(long id);
    }
}
