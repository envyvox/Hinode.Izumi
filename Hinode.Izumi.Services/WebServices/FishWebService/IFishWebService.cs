using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.FishWebService.Models;

namespace Hinode.Izumi.Services.WebServices.FishWebService
{
    public interface IFishWebService
    {
        Task<IEnumerable<FishWebModel>> GetAllFish();
        Task<FishWebModel> Get(long id);
        Task<FishWebModel> Update(FishWebModel model);
        Task Remove(long id);
    }
}
