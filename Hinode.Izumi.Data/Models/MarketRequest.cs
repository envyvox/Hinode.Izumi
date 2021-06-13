using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Models.UserModels;

namespace Hinode.Izumi.Data.Models
{
    public class MarketRequest : EntityBase
    {
        public long UserId { get; set; }
        public MarketCategory Category { get; set; }
        public long ItemId { get; set; }
        public long Price { get; set; }
        public long Amount { get; set; }
        public bool Selling { get; set; }
        public virtual User User { get; set; }
    }
}
