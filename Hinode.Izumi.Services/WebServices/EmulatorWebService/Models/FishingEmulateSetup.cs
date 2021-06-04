using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.WebServices.EmulatorWebService.Models
{
    public class FishingEmulateSetup
    {
        public TimesDay TimesDay { get; set; }
        public Season Season { get; set; }
        public Weather Weather { get; set; }
        public double FishingMastery { get; set; }
        public bool MasteryStackable { get; set; }
        public long FishingCount { get; set; }
        public long EmulateCount { get; set; }
    }
}
