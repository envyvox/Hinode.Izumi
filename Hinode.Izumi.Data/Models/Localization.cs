using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class Localization : EntityBase
    {
        public LocalizationCategory Category { get; set; }
        public long ItemId { get; set; }
        public string Name { get; set; }
        public string Single { get; set; }
        public string Double { get; set; }
        public string Multiply { get; set; }
    }
}
