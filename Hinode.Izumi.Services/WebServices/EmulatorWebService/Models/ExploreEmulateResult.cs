using System.Collections.Generic;

namespace Hinode.Izumi.Services.WebServices.EmulatorWebService.Models
{
    public class ExploreEmulateResult
    {
        public decimal AverageSuccessPercent { get; set; }
        public double AverageMasteryReceived { get; set; }
        public double AverageCurrencyReceived { get; set; }
        public Dictionary<long, ExploreResult> ExploreResults { get; set; }
    }

    public class ExploreResult
    {
        public long SuccessCount { get; set; }
        public long FailCount { get; set; }
        public decimal SuccessPercent { get; set; }
        public double MasteryReceived { get; set; }
        public long CurrencyReceived { get; set; }
        public Dictionary<string, long> GatheringsReceived { get; set; }
        public long TotalItemsCount { get; set; }
    }
}
