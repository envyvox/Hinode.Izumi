namespace Hinode.Izumi.Services.GameServices.InventoryService.Records
{
    public record UserDrinkRecord(
        long UserId,
        long DrinkId,
        long Amount,
        string Name)
    {
        public UserDrinkRecord() : this(default, default, default, default)
        {
        }
    }
}
