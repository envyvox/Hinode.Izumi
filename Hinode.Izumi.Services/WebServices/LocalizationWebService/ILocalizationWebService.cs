using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.LocalizationWebService.Models;

namespace Hinode.Izumi.Services.WebServices.LocalizationWebService
{
    public interface ILocalizationWebService
    {
        Task<IEnumerable<LocalizationWebModel>> GetAllLocalizations();
        Task<LocalizationWebModel> Get(long id);
        Task<LocalizationWebModel> Update(LocalizationWebModel model);
        Task Upload();
    }
}
