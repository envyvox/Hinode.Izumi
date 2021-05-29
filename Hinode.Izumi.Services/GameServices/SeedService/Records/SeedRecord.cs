using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.SeedService.Records
{
    public record SeedRecord(
        long Id,
        string Name,
        Season Season,
        long Growth,
        long ReGrowth,
        long Price,
        bool Multiply)
    {
        public SeedRecord() : this(default, default, default, default, default, default, default)
        {
        }
    }
}
