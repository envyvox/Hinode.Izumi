using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.AchievementWebService.Models;

namespace Hinode.Izumi.Services.WebServices.AchievementWebService
{
    public interface IAchievementWebService
    {
        Task<IEnumerable<AchievementWebModel>> GetAllAchievements();
        Task<AchievementWebModel> Get(long id);
        Task<AchievementWebModel> Update(AchievementWebModel model);
        Task Upload();
    }
}
