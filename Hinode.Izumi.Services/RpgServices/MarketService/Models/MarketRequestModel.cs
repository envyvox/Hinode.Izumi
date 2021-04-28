using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.MarketService.Models
{
    /// <summary>
    /// Заявка на рынке.
    /// </summary>
    public class MarketRequestModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Категория товара.
        /// </summary>
        public MarketCategory Category { get; set; }

        /// <summary>
        /// Id товара.
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Цена товара.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Количество товара.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Выставлен на продажу?
        /// </summary>
        public bool Selling { get; set; }
    }
}
