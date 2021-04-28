using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.EmoteWebService.Models;

namespace Hinode.Izumi.Services.WebServices.EmoteWebService
{
    public interface IEmoteWebService
    {
        Task<IEnumerable<EmoteWebModel>> GetAllEmotes();
        Task<EmoteWebModel> Get(long id);
        Task UploadEmotes();
    }
}
