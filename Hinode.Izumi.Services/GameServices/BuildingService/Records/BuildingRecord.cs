using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Records
{
    public record BuildingRecord(
        long Id,
        Building Type,
        long? ProjectId,
        BuildingCategory Category,
        string Name,
        string Description)
    {
        public BuildingRecord() : this(default, default, default, default, default, default)
        {
        }
    }
}
