namespace Hinode.Izumi.Data.Models
{
    public class DynamicShopBanner : EntityBase
    {
        public long BannerId { get; set; }
        public virtual Banner Banner { get; set; }
    }
}
