using System.Collections.Generic;

namespace Hinode.Izumi.Services.WebServices.EmulatorWebService.Models
{
    public class FishingEmulateResult
    {
        public decimal AverageSuccessPercent { get; set; }
        public double AverageMasteryReceived { get; set; }
        public double AverageCurrencyReceived { get; set; }
        public Dictionary<long, FishingResult> FishingResults { get; set; }
    }

    public class FishingResult
    {
        public long SuccessCount { get; set; }
        public long FailCount { get; set; }
        public decimal SuccessPercent { get; set; }
        public double MasteryReceived { get; set; }
        public long CurrencyReceived { get; set; }
        public long CommonFishCount { get; set; }
        public long RareFishCount { get; set; }
        public long EpicFishCount { get; set; }
        public long MythicalFishCount { get; set; }
        public long LegendaryFishCount { get; set; }
        public long DivineFishCount { get; set; }
    }
}
