using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.GatheringService.Records
{
    public record GatheringRecord(long Id, string Name, long Price, Location Location, Event Event)
    {
        public GatheringRecord() : this(default, default, default, default, default)
        {
        }
    }
}
