using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.ImageWebService.Models;

namespace Hinode.Izumi.Services.WebServices.ImageWebService
{
    public interface IImageWebService
    {
        Task<IEnumerable<ImageWebModel>> GetAllImages();
        Task<ImageWebModel> Get(long id);
        Task<ImageWebModel> Update(ImageWebModel model);
        Task Upload();
    }
}
