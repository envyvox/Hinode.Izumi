using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.ProductWebService.Models;

namespace Hinode.Izumi.Services.WebServices.ProductWebService
{
    public interface IProductWebService
    {
        Task<IEnumerable<ProductWebModel>> GetAllProducts();
        Task<ProductWebModel> Get(long id);
        Task<ProductWebModel> Update(ProductWebModel model);
        Task Remove(long id);
    }
}
