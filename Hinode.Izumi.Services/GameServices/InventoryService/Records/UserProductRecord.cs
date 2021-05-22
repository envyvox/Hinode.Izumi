namespace Hinode.Izumi.Services.GameServices.InventoryService.Records
{
    public record UserProductRecord(
        long UserId,
        long ProductId,
        long Amount,
        string Name)
    {
        public UserProductRecord() : this(default, default, default, default)
        {
        }
    }
}
