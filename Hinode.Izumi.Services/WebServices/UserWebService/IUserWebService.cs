using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.UserWebService.Models;

namespace Hinode.Izumi.Services.WebServices.UserWebService
{
    public interface IUserWebService
    {
        Task<IEnumerable<UserWebModel>> GetAllUsers();
        Task<UserWebModel> Get(long id);
        Task<UserWebModel> Update(UserWebModel model);
        Task Remove(long id);
    }
}
