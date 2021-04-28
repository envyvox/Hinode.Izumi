using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.SeedWebService.Models;

namespace Hinode.Izumi.Services.WebServices.SeedWebService
{
    public interface ISeedWebService
    {
        Task<IEnumerable<SeedWebModel>> GetAllSeeds();
        Task<SeedWebModel> Get(long id);
        Task<SeedWebModel> Upsert(SeedWebModel model);
        Task Remove(long id);
    }
}
