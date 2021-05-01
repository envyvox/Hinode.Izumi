using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.CropWebService.Models;

namespace Hinode.Izumi.Services.WebServices.CropWebService
{
    public interface ICropWebService
    {
        Task<IEnumerable<CropWebModel>> GetAllCrops();
        Task<CropWebModel> Get(long id);
        Task<CropWebModel> Upsert(CropWebModel model);
        Task Remove(long id);
    }
}
