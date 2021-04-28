namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Продукт.
    /// </summary>
    public class Product : EntityBase
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
