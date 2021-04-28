using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.GatheringWebService.Models;

namespace Hinode.Izumi.Services.WebServices.GatheringWebService
{
    public interface IGatheringWebService
    {
        Task<IEnumerable<GatheringWebModel>> GetAllGathering();
        Task<GatheringWebModel> Get(long id);
        Task<GatheringWebModel> Update(GatheringWebModel model);
        Task Remove(long id);
    }
}
