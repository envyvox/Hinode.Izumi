using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.ProductWebService.Models
{
    /// <summary>
    /// Продукт.
    /// </summary>
    public class ProductWebModel : EntityBaseModel
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
