namespace Hinode.Izumi.Services.GameServices.InventoryService.Records
{
    public record UserAlcoholRecord(
        long UserId,
        long AlcoholId,
        long Amount,
        string Name)
    {
        public UserAlcoholRecord() : this(default, default, default, default)
        {
        }
    }
}
