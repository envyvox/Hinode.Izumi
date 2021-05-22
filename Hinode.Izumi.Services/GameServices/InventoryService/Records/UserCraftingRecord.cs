namespace Hinode.Izumi.Services.GameServices.InventoryService.Records
{
    public record UserCraftingRecord(
        long UserId,
        long CraftingId,
        long Amount,
        string Name)
    {
        public UserCraftingRecord() : this(default, default, default, default)
        {
        }
    }
}
