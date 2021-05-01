using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.AlcoholIngredientWebService.Models;

namespace Hinode.Izumi.Services.WebServices.AlcoholIngredientWebService
{
    public interface IAlcoholIngredientWebService
    {
        Task<IEnumerable<AlcoholIngredientWebModel>> GetAllAlcoholIngredients();
        Task<IEnumerable<AlcoholIngredientWebModel>> GetAlcoholIngredients(long alcoholId);
        Task<AlcoholIngredientWebModel> Get(long id);
        Task<AlcoholIngredientWebModel> Upsert(AlcoholIngredientWebModel model);
        Task Remove(long id);
    }
}
