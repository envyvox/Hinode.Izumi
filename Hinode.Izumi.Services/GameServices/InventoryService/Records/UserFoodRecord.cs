namespace Hinode.Izumi.Services.GameServices.InventoryService.Records
{
    public record UserFoodRecord(
        long UserId,
        long FoodId,
        long Amount,
        string Name,
        long Mastery)
    {
        public UserFoodRecord() : this(default, default, default, default, default)
        {
        }
    }
}
