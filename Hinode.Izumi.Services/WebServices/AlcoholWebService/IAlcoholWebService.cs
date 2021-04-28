using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.AlcoholWebService.Models;

namespace Hinode.Izumi.Services.WebServices.AlcoholWebService
{
    public interface IAlcoholWebService
    {
        Task<IEnumerable<AlcoholWebModel>> GetAllAlcohols();
        Task<AlcoholWebModel> Get(long id);
        Task<AlcoholWebModel> Update(AlcoholWebModel model);
        Task Remove(long id);
    }
}
