namespace Hinode.Izumi.Services.GameServices.InventoryService.Records
{
    public record UserGatheringRecord(
        long UserId,
        long GatheringId,
        long Amount,
        string Name)
    {
        public UserGatheringRecord() : this(default, default, default, default)
        {
        }
    }
}
