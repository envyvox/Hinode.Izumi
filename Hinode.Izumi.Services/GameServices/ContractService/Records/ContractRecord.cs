using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.ContractService.Records
{
    public record ContractRecord(
        long Id,
        Location Location,
        string Name,
        string Description,
        long Time,
        long Currency,
        long Reputation,
        long Energy)
    {
        public ContractRecord() : this(default, default, default, default, default, default, default, default)
        {
        }
    }
}
