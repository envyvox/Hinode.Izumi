using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.WebServices.EmulatorWebService.Models
{
    public class ExploreEmulateSetup
    {
        public Location Location { get; set; }
        public double GatheringMastery { get; set; }
        public bool MasteryStackable { get; set; }
        public long ExploreCount { get; set; }
        public long EmulateCount { get; set; }
    }
}
