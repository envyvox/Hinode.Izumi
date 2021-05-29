using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Records
{
    public record FamilyCurrencyRecord(long FamilyId, Currency Currency, long Amount)
    {
        public FamilyCurrencyRecord() : this(default, default, default)
        {
        }
    }
}
