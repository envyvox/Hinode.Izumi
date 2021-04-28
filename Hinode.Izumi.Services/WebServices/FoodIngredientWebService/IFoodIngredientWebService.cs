using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.FoodIngredientWebService.Models;

namespace Hinode.Izumi.Services.WebServices.FoodIngredientWebService
{
    public interface IFoodIngredientWebService
    {
        Task<IEnumerable<FoodIngredientWebModel>> GetAllFoodIngredients();
        Task<IEnumerable<FoodIngredientWebModel>> GetFoodIngredients(long foodId);
        Task<FoodIngredientWebModel> Get(long id);
        Task<FoodIngredientWebModel> Update(FoodIngredientWebModel model);
        Task Remove(long id);
    }
}
