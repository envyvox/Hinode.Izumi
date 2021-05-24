namespace Hinode.Izumi.Services.GameServices.FoodService.Records
{
    public record FoodRecord(long Id, string Name, long Mastery, long Time)
    {
        public FoodRecord() : this(default, default, default, default)
        {
        }
    }
}
