using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.AlcoholPropertyWebService.Models;

namespace Hinode.Izumi.Services.WebServices.AlcoholPropertyWebService
{
    public interface IAlcoholPropertyWebService
    {
        Task<IEnumerable<AlcoholPropertyWebModel>> GetAllAlcoholProperties();
        Task<IEnumerable<AlcoholPropertyWebModel>> GetAlcoholProperties(long alcoholId);
        Task<AlcoholPropertyWebModel> Get(long id);
        Task<AlcoholPropertyWebModel> Update(AlcoholPropertyWebModel model);
        Task Upload();
    }
}
