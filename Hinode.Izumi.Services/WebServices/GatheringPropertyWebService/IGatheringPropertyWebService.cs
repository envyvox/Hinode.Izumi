using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.GatheringPropertyWebService.Models;

namespace Hinode.Izumi.Services.WebServices.GatheringPropertyWebService
{
    public interface IGatheringPropertyWebService
    {
        Task<IEnumerable<GatheringPropertyWebModel>> GetAllGatheringProperties();
        Task<GatheringPropertyWebModel> Get(long id);
        Task<GatheringPropertyWebModel> Update(GatheringPropertyWebModel model);
        Task Upload();
    }
}
