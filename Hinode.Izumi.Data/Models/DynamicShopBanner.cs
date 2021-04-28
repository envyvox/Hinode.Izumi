namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Баннер в динамическом магазине.
    /// </summary>
    public class DynamicShopBanner : EntityBase
    {
        /// <summary>
        /// Id баннера.
        /// </summary>
        public long BannerId { get; set; }

        /// <summary>
        /// Баннер.
        /// </summary>
        public virtual Banner Banner { get; set; }
    }
}
