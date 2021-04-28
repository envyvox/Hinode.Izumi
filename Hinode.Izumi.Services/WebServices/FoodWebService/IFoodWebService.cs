using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.FoodWebService.Models;

namespace Hinode.Izumi.Services.WebServices.FoodWebService
{
    public interface IFoodWebService
    {
        Task<IEnumerable<FoodWebModel>> GetAllFood();
        Task<FoodWebModel> Get(long id);
        Task<FoodWebModel> Update(FoodWebModel model);
        Task Remove(long id);
    }
}
