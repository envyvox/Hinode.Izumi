using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.CropService.Records
{
    public record CropRecord(long Id, string Name, long Price, long SeedId, Season Season)
    {
        public CropRecord() : this(default, default, default, default, default)
        {
        }
    }
}
