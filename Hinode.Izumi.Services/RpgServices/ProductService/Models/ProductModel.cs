using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.ProductService.Models
{
    /// <summary>
    /// Продукт.
    /// </summary>
    public class ProductModel : EntityBaseModel
    {
        /// <summary>
        /// Название продукта.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Цена продукта.
        /// </summary>
        public long Price { get; set; }
    }
}
