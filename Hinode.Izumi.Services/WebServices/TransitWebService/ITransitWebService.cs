using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.TransitWebService.Models;

namespace Hinode.Izumi.Services.WebServices.TransitWebService
{
    public interface ITransitWebService
    {
        Task<IEnumerable<TransitWebModel>> GetAllTransits();
        Task<TransitWebModel> Get(long id);
        Task<TransitWebModel> Update(TransitWebModel model);
        Task Remove(long id);
    }
}
