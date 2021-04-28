using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.MasteryPropertyWebService.Models;

namespace Hinode.Izumi.Services.WebServices.MasteryPropertyWebService
{
    public interface IMasteryPropertyWebService
    {
        Task<IEnumerable<MasteryPropertyWebModel>> GetAllProperties();
        Task<MasteryPropertyWebModel> Get(long id);
        Task<MasteryPropertyWebModel> Update(MasteryPropertyWebModel model);
        Task Upload();
    }
}
