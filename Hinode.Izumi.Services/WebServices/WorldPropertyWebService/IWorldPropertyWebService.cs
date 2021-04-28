using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.WorldPropertyWebService.Models;

namespace Hinode.Izumi.Services.WebServices.WorldPropertyWebService
{
    public interface IWorldPropertyWebService
    {
        Task<IEnumerable<WorldPropertyWebModel>> GetAllProperties();
        Task<WorldPropertyWebModel> Get(long id);
        Task<WorldPropertyWebModel> Update(WorldPropertyWebModel model);
        Task Upload();
    }
}
