using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.DrinkWebService.Models;

namespace Hinode.Izumi.Services.WebServices.DrinkWebService
{
    public interface IDrinkWebService
    {
        Task<IEnumerable<DrinkWebModel>> GetAllDrinks();
        Task<DrinkWebModel> Get(long id);
        Task<DrinkWebModel> Update(DrinkWebModel model);
        Task Remove(long id);
    }
}
