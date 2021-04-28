using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Models.UserModels;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Заявка на рынке.
    /// </summary>
    public class MarketRequest : EntityBase
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

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }
    }
}
