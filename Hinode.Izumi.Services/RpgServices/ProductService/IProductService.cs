using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.ProductService.Models;

namespace Hinode.Izumi.Services.RpgServices.ProductService
{
    public interface IProductService
    {
        /// <summary>
        /// Возвращает массив продуктов.
        /// </summary>
        /// <returns>Массив продуктов.</returns>
        Task<ProductModel[]> GetAllProducts();

        /// <summary>
        /// Возвращает продукт. Кэшируется.
        /// </summary>
        /// <param name="id">Id продукта.</param>
        /// <returns>Продукт.</returns>
        /// <exception cref="IzumiNullableMessage.Product"></exception>
        Task<ProductModel> GetProduct(long id);
    }
}
